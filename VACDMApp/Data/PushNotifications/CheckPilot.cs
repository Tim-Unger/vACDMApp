using Android.App;
using Android.App.Admin;
using VACDMApp.Data.OverridePermissions;
using VACDMApp.VACDMData;
using static VACDMApp.Data.PushNotifications.PushNotificationHandler;

namespace VACDMApp.Data.PushNotifications
{
    internal class PushHandler
    {
        private static List<(NotificationType notificationType, TimeOnly pushTime, DateTime? pushedTsat)> _pushedNotifications =
            new();

        private static DateTime _lastTsat;

        private static readonly Settings _settings = VACDMData.Data.Settings;

        internal static async Task CheckPilotAsync(VACDMPilot pilot)
        {
            var grantState = await Permissions.CheckStatusAsync<SendNotifications>();

            if (grantState != PermissionStatus.Granted)
            {
                //We can't send notifications, so there is no point to continue
                return;
            }

            var ownFlight = VACDMData.Data.VatsimPilots.FirstOrDefault(x => x.cid == _settings.Cid);

            var isOwnFlight = false;

            if (
                ownFlight is not null
                && VACDMData.Data.VACDMPilots.Any(x => x.Callsign == ownFlight.callsign)
            )
            {
                isOwnFlight = true;
            }

            var vacdm = pilot.Vacdm;

            var pushTime = TimeOnly.FromDateTime(DateTime.UtcNow);

            //Startup in the Window now
            if (vacdm.Tsat.IsTsatInTheWindow())
            {
                if (!IsAllowedToSendPush(NotificationType.SlotNow, isOwnFlight))
                {
                    return;
                }

                //Slot Now Push has already been sent
                if (_pushedNotifications.Any(x => x.notificationType == NotificationType.SlotNow))
                {
                    return;
                }

                _pushedNotifications.Add(new(NotificationType.SlotNow, pushTime, null));

                await PushSender.SendPushNotificationAsync(pilot, NotificationType.SlotNow);

                return;
            }

            //Slot changed
            //Check if the TSAT is different from the last time we checked
            if (_lastTsat != vacdm.Tsat)
            {
                if (!IsAllowedToSendPush(NotificationType.SlotChanged, isOwnFlight))
                {
                    return;
                }

                //When we are checking for the first time, we need to set the initial TSAT to prevent triggering the Push immediately
                if (_lastTsat == DateTime.MinValue)
                {
                    _lastTsat = vacdm.Tsat;
                    return;
                }

                //The changed Slot Push for this Slot revision has already been sent
                if (
                    _pushedNotifications
                        .Where(x => x.notificationType == NotificationType.SlotChanged)
                        .Any(x => x.pushedTsat == vacdm.Tsat)
                )
                {
                    return;
                }

                _pushedNotifications.Add(new(NotificationType.SlotChanged, pushTime, vacdm.Tsat));
                _lastTsat = vacdm.Tsat;

                await PushSender.SendPushNotificationAsync(pilot, NotificationType.SlotChanged);

                return;
            }

            //Startup Received
            if (vacdm.Sug != DateTime.MinValue)
            {
                if (!IsAllowedToSendPush(NotificationType.StartupGiven, isOwnFlight))
                {
                    return;
                }

                //Startup Push has already been sent
                if (
                    _pushedNotifications.Any(
                        x => x.notificationType == NotificationType.StartupGiven
                    )
                )
                {
                    return;
                }

                _pushedNotifications.Add(new(NotificationType.StartupGiven, pushTime, null));

                await PushSender.SendPushNotificationAsync(pilot, NotificationType.StartupGiven);

                return;
            }

            //Slot starts within the next 5 minutes (so 10 min. from the TSAT) and is not confirmed
            if (vacdm.Tsat.AddMinutes(-10) < DateTime.UtcNow)
            {
                //Unconfirmed Slot Push has already been given
                if (
                    _pushedNotifications.Any(
                        x => x.notificationType == NotificationType.SlotSoonUnconfirmed
                    )
                )
                {
                    return;
                }

                if (vacdm.TobtState == "GUESS")
                {
                    _pushedNotifications.Add(
                        new(NotificationType.SlotSoonUnconfirmed, pushTime, null)
                    );

                    await PushSender.SendPushNotificationAsync(
                        pilot,
                        NotificationType.SlotSoonUnconfirmed
                    );
                    return;
                }

                return;
            }
        }

        private static bool IsAllowedToSendPush(NotificationType notificationType, bool isOwnFlight)
        {
            if (isOwnFlight)
            {
                return notificationType switch
                {
                    NotificationType.SlotNow => _settings.SendPushMyFlightInsideWindow,
                    NotificationType.SlotChanged => _settings.SendPushMyFlightTsatChanged,
                    NotificationType.StartupGiven => _settings.SendPushMyFlightStartup,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return notificationType switch
            {
                NotificationType.SlotNow => _settings.SendPushBookmarkFlightInsideWindow,
                NotificationType.SlotChanged => _settings.SendPuishBookmarkTsatChanged,
                NotificationType.StartupGiven => _settings.SendPushBookmarkStartup,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
