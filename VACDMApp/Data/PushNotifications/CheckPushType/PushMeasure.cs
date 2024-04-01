using static VacdmApp.Data.PushNotifications.PushNotificationHandler;

namespace VacdmApp.Data.PushNotifications
{
    internal partial class PushType
    {
        internal static async Task PushNewMeasureAsync(FlowMeasure flowMeasure)
        {
            if (!IsAllowedToSendPush(NotificationType.NewFlowMeasure))
            {
                return;
            }

            var firsToPush = JsonSerializer.Deserialize<IEnumerable<string>>(CurrentSettings.FlowMeasurePushFirs);

            //Check if Measure concerns us
            if (!firsToPush.Any(x => flowMeasure.NotifiedFirs.Any(y => y.Identifier == x)))
            {
                return;
            }

            await PushSender.SendPushNotificationAsync(flowMeasure, NotificationType.NewFlowMeasure);
        }

        internal static async Task PushActiveMeasureAsync(FlowMeasure flowMeasure)
        {
            //TODO
            await PushSender.SendPushNotificationAsync(flowMeasure, NotificationType.ActiveFlowMeasure);
        }

        internal static async Task PushExpiredMeasureAsync(FlowMeasure flowMeasure)
        {
            await PushSender.SendPushNotificationAsync(flowMeasure, NotificationType.ExpiredFlowMeasure);
        }

        internal static async Task PushWithdrawnMeasure(FlowMeasure flowMeasure)
        {
            await PushSender.SendPushNotificationAsync(flowMeasure, NotificationType.WithdrawnFlowMeasure);
        }
    }
}
