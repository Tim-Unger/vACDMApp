using Plugin.LocalNotification;
using static VacdmApp.Data.PushNotifications.PushNotificationHandler;
using VacdmApp.Data;

namespace VacdmApp.Data.PushNotifications
{
    internal class PushSender
    {
        internal static List<(NotificationRequest request, string callsign)> SentNotifications =
            new();

        internal static async Task SendPushNotificationAsync(
            VacdmPilot pilot,
            NotificationType notificationType
        )
        {
            var notificationMessageString = notificationType switch
            {
                NotificationType.SlotNow
                  => $"{pilot.Callsign} is now in the Startup-Window. Your Slot will have expired when this message is {TimeToExpire(pilot)} min. old",
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

            var random = new Random();

            var randomId = random.Next();

            var request = new NotificationRequest()
            {
                NotificationId = randomId,
                Title = notificationMessageTitle,
                Subtitle = "vACDM",
                Description = notificationMessageString,
                //Image = new NotificationImage() { ResourceName = "splash.svg"}
            };

            SentNotifications.Add(new(request, pilot.Callsign));

            await LocalNotificationCenter.Current.Show(request);
        }

        private static int TimeToExpire(VacdmPilot pilot)
        {
            var timeDifference = pilot.Vacdm.Tsat.AddMinutes(5) - DateTime.UtcNow;

            var totalMinutes = timeDifference.TotalMinutes;

            var roundMinutes = Math.Round(totalMinutes, 0);

            return int.Parse(roundMinutes.ToString());
        }
    }
}
