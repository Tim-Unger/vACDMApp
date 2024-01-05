using static VACDMApp.Data.PushNotifications.PushNotificationHandler;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.PushNotifications
{
    internal partial class PushType
    {
        internal static async Task PushStartupReceivedAsync(VACDMPilot pilot, bool isOwnFlight, TimeOnly pushTime)
        {
            if (!IsAllowedToSendPush(NotificationType.StartupGiven, isOwnFlight))
            {
                return;
            }

            //Startup Push has already been sent
            if (
                PushedNotifications
                    .Where(x => x.callsign == pilot.Callsign)
                    .Any(x => x.notificationType == NotificationType.StartupGiven)
            )
            {
                return;
            }

            PushedNotifications.Add(
                new(pilot.Callsign, NotificationType.StartupGiven, pushTime, null)
            );

            await PushSender.SendPushNotificationAsync(pilot, NotificationType.StartupGiven);

            return;
        }
    }
}
