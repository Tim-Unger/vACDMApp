using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace VacdmApp.Data.PushNotifications
{
    internal partial class PushNotificationHandler
    {
        //private static List<(string callsign, DateTime pushedTime)> PushedPilots = new();

        private static readonly List<VacdmPilot> _subscribedPilots = new();

        private static readonly List<FlowMeasure> _checkedMeasures = new();

        internal enum NotificationType
        {
            SlotNow,
            SlotChanged,
            StartupGiven,
            SlotSoonUnconfirmed,
            FlowMeasure //TODO Flow Measure Push
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
                var tasks = _subscribedPilots.Select(CheckPilotAsync).ToList();
                tasks.Add(CheckFlowMeasuresAsyc());

                await Task.WhenAll(tasks);

                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }

        //TODO
        private static async Task CheckFlowMeasuresAsyc()
        {
            var measures = Data.FlowMeasures;

            var differences = _checkedMeasures.Where(
                w =>
                    !measures.Any(
                        x =>
                            w.NotifiedFirs.Any(
                                y => x.NotifiedFirs.Any(z => z.Identifier == y.Identifier)
                            )
                    )
            );

            if (!differences.Any())
            {
                return;
            }

            await Task.Delay(10);
        }

        internal static async Task SubscribeAsync(VacdmPilot pilot)
        {
            _subscribedPilots.Add(pilot);

            await CheckPilotAsync(pilot);
        }

        internal static bool Unsubscribe(VacdmPilot pilot)
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
