using static VACDMApp.Data.PushNotifications.PushNotificationHandler;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.PushNotifications
{
    internal partial class PushType
    {
        internal static async Task PushSlotUnconfirmed(VACDMPilot pilot, bool isOwnFlight, TimeOnly pushTime)
        {
            if (!IsAllowedToSendPush(NotificationType.StartupGiven, isOwnFlight))
            {
                return;
            }

            //You can only confirm you own slot, so every other flight doesn't make sense
            if (!isOwnFlight)
            {
                return;
            }

            var vacdm = pilot.Vacdm;

            //Unconfirmed Slot Push has already been given
            if (
                PushedNotifications
                    .Where(x => x.callsign == pilot.Callsign)
                    .Any(x => x.notificationType == NotificationType.SlotSoonUnconfirmed)
            )
            {
                return;
            }

            if (vacdm.TobtState == "GUESS")
            {
                PushedNotifications.Add(
                    new(pilot.Callsign, NotificationType.SlotSoonUnconfirmed, pushTime, null)
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
}
