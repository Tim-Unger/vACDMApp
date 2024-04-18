using VacdmApp.Data.OverridePermissions;
namespace VacdmApp.Data.PushNotifications
{
    internal static partial class PushNotificationHandler
    {
        internal static List<(string callsign, NotificationType notificationType, TimeOnly pushTime, DateTime? pushedTsat)> PushedNotifications =
            new();

        internal static readonly Settings CurrentSettings = Data.Settings;

        internal static List<(string callsign, DateTime lastTsat)> LastTsats = new();

        internal static async Task CheckPilotAsync(VacdmPilot pilot)
        {
            //Pilot has disconnected or been removed from the vACDM Pilots
            if (!Data.VacdmPilots.Any(x => x.Callsign == pilot.Callsign))
            {
                return;
            }

            var grantState = await Permissions.CheckStatusAsync<SendNotifications>();

            if (grantState != PermissionStatus.Granted)
            {
                //We can't send notifications, so there is no point to continue
                return;
            }

            var ownFlight = Data.VatsimPilots.FirstOrDefault(x => x.cid == CurrentSettings.Cid);

            var isOwnFlight = false;

            if (
                ownFlight is not null
                && Data.VacdmPilots.Any(x => x.Callsign == ownFlight.callsign)
            )
            {
                isOwnFlight = true;
            }

            var vacdm = pilot.Vacdm;

            var pushTime = TimeOnly.FromDateTime(DateTime.UtcNow);

            //Startup in the Window now
            if (vacdm.Tsat.IsTsatInTheWindow())
            {
                await PushType.PushSlotNowAsync(pilot, isOwnFlight, pushTime);
                return;
            }

            //When we are checking for the first time, we need to set the initial TSAT to prevent triggering the Push immediately
            if (!LastTsats.Exists(x => x.callsign == pilot.Callsign))
            {
                LastTsats.Add(new(pilot.Callsign, vacdm.Tsat));
                return;
            }

            //Slot changed
            //Check if the TSAT is different from the last time we checked
            if (LastTsats.First(x => x.callsign == pilot.Callsign).lastTsat != vacdm.Tsat)
            {
                await PushType.PushSlotChangedAsync(pilot, isOwnFlight, pushTime);
                return;
            }

            //Startup Received
            if (vacdm.Sug != DateTime.MinValue)
            {
                await PushType.PushStartupReceivedAsync(pilot, isOwnFlight, pushTime);
                return;
            }

            //Slot starts within the next 5 minutes (so 10 min. from the TSAT) and is not confirmed
            if (vacdm.Tsat.AddMinutes(-10) > DateTime.UtcNow)
            {
                await PushType.PushSlotUnconfirmed(pilot, isOwnFlight, pushTime);
                return;
            }
        }
    }
}