using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.PushNotifications
{
    internal class PushNotificationHandler
    {
        private static List<(string callsign, DateTime pushedTime)> PushedPilots = new();

        internal enum NotificationType
        {
            SlotNow,
            SlotChanged,
            StartupGiven,
            SlotSoonUnconfirmed,
            NewFlowMeasure //TODO Flow Measure Push
        }

        //TODO implement
        internal static void InitializeNotificationEvents(INotificationService notificationService)
        {
            notificationService.NotificationActionTapped += NotificationTapped;
        }

        private static void NotificationTapped(NotificationActionEventArgs e)
        {
            var title = e.Request.Title;
        }

        internal static async Task SendPushNotificationAsync(
            VACDMPilot pilot,
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

            var request = new NotificationRequest()
            {
                Title = notificationMessageTitle,
                Subtitle = "vACDM",
                Description = notificationMessageString,
            };

            await LocalNotificationCenter.Current.Show(request);
        }

        internal static async Task CheckTimeWindowAndPushMessage(List<VACDMPilot> pilots)
        {
            foreach (var pilot in pilots)
            {
                var vacdm = pilot.Vacdm;

                if (!vacdm.Tsat.IsTsatInTheWindow())
                {
                    return;
                }

                if (vacdm.Tsat.IsTsatInTheWindow() && !PushedPilots.Exists(x => x.callsign == pilot.Callsign))
                {
                    await SendPushNotificationAsync(pilot, NotificationType.SlotNow);

                    PushedPilots.Add(new(pilot.Callsign, vacdm.Tsat));

                    return;
                }

                var pushedPilot = PushedPilots.First(x => x.callsign == pilot.Callsign);

                if (pushedPilot.pushedTime != vacdm.Tsat)
                {
                    await SendPushNotificationAsync(pilot, NotificationType.SlotChanged);

                    var pushIndex = PushedPilots.FindIndex(x => x.callsign == pilot.Callsign);
                    PushedPilots[pushIndex] = new(pushedPilot.callsign, vacdm.Tsat);
                }

                //TODO Startup Given
            }
        }

        private static int TimeToExpire(VACDMPilot pilot) 
        {
            var timeDifference = pilot.Vacdm.Tsat.AddMinutes(5) - DateTime.UtcNow;

            var totalMinutes = timeDifference.TotalMinutes;

            var roundMinutes = Math.Round(totalMinutes, 0);

            return int.Parse(roundMinutes.ToString());
        }
    }
}