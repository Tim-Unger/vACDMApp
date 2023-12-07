using Plugin.LocalNotification;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.PushNotifications
{
    internal class PushNotificationHandler
    { 
        internal enum NotificationType
        {
            SlotNow,
            SlotChanged,
            StartupGiven,
            SlotSoonUnconfirmed
        }

        internal static async Task SendPushNotificationAsync(
            VACDMPilot pilot,
            NotificationType notificationType
        )
        {
            var notificationMessageString = notificationType switch
            {
                NotificationType.SlotNow
                  => $"Your Flight {pilot.Callsign} is now in the Startup-Window.\rTSAT {pilot.Vacdm.Tsat.ToShortTimeString()}Z\rYour Slot will have expired when this message is 10 min. old",
                NotificationType.SlotChanged
                  => $"TSAT changed for your Flight {pilot.Callsign}\r New TSAT {pilot.Vacdm.Tsat.ToShortTimeString()}Z",
                NotificationType.StartupGiven
                  => $"Your Flight {pilot.Callsign} has just received Startup.\rEnjoy your Flight!",
                NotificationType.SlotSoonUnconfirmed
                  => $"The Slot for your Flight {pilot.Callsign} is soon but not yet confirmed.\rConfirm your Slot to make sure you won't get delayed.",
                _ => throw new ArgumentOutOfRangeException()
            };

            var notificationMessageTitle = notificationType switch
            {
                NotificationType.SlotNow => $"{pilot.Callsign} Slot Now",
                NotificationType.SlotChanged => $"{pilot.Callsign} Slot Changed",
                NotificationType.StartupGiven => $"{pilot.Callsign} Startup received",
                NotificationType.SlotSoonUnconfirmed => $"{pilot.Callsign} Slot not yet confirmed",
                _ => throw new ArgumentOutOfRangeException()
            };

             var request = new NotificationRequest()
             {
                 Title = notificationMessageTitle,
                 Subtitle = "vACDM",
                 Description = notificationMessageString,
             };

            await LocalNotificationCenter.Current.Show(request);
        }
    }
}
