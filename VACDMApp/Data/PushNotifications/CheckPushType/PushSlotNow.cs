using static VACDMApp.Data.PushNotifications.PushNotificationHandler;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.PushNotifications
{
    internal partial class PushType
    {
        internal static async Task PushSlotNowAsync(VACDMPilot pilot, bool isOwnFlight, TimeOnly pushTime)
        {
            if (!IsAllowedToSendPush(NotificationType.SlotNow, isOwnFlight))
            {
                return;
            }

            //Slot Now Push has already been sent
            if (
                PushedNotifications
                    .Where(x => x.callsign == pilot.Callsign)
                    .Any(x => x.notificationType == NotificationType.SlotNow)
            )
            {
                return;
            }

            PushedNotifications.Add(
                new(pilot.Callsign, NotificationType.SlotNow, pushTime, null)
            );

            await PushSender.SendPushNotificationAsync(pilot, NotificationType.SlotNow);

            return;
        }

    }
}
