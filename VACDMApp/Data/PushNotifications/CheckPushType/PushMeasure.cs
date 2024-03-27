using static VacdmApp.Data.PushNotifications.PushNotificationHandler;

namespace VacdmApp.Data.PushNotifications
{
    internal partial class PushType
    {
        internal static async Task PushNewMeasureAsync(FlowMeasure flowMeasure)
        {
            if (!IsAllowedToSendPush(NotificationType.FlowMeasure))
            {
                return;
            }

            var firsToPush = JsonSerializer.Deserialize<IEnumerable<string>>(CurrentSettings.FlowMeasurePushFirs);

            //Check if Measure concerns us
            if (!firsToPush.Any(x => flowMeasure.NotifiedFirs.Any(y => y.Identifier == x)))
            {
                return;
            }

            //TODO
        }
    }
}
