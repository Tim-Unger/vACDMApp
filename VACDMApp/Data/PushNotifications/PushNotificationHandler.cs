using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.PushNotifications
{
    internal partial class PushNotificationHandler
    {
        //private static List<(string callsign, DateTime pushedTime)> PushedPilots = new();

        private static List<VACDMPilot> _subscribedPilots = new();

        internal enum NotificationType
        {
            SlotNow,
            SlotChanged,
            StartupGiven,
            SlotSoonUnconfirmed,
            NewFlowMeasure //TODO Flow Measure Push
        }

        //TODO implement
        internal static async Task InitializeNotificationEvents(
            INotificationService notificationService
        )
        {
            notificationService.NotificationActionTapped += NotificationTapped;
        }

        private static void NotificationTapped(NotificationActionEventArgs e) =>
            OpenPush.OpenTapped(e);

        internal static async Task StartGlobalHandler()
        {
            while (true)
            {
                var tasks = _subscribedPilots.Select(CheckPilotAsync);

                await Task.WhenAll(tasks);

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }

        internal static async Task SubscribeAsync(VACDMPilot pilot)
        {
            _subscribedPilots.Add(pilot);

            await CheckPilotAsync(pilot);
        }

        internal static bool Unsubscribe(VACDMPilot pilot)
        {
            var concernedPilot = _subscribedPilots.FirstOrDefault(
                x => x.Callsign == pilot.Callsign
            );

            if (concernedPilot is null)
            {
                return false;
            }

            var index = _subscribedPilots.FindIndex(x => x.Callsign == pilot.Callsign);

            _subscribedPilots.RemoveAt(index);

            ClearPush.ClearAll(pilot.Callsign);

            return true;
        }
    }
}
