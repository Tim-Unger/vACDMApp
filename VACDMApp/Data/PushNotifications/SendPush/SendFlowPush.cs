using Plugin.LocalNotification;
using static VacdmApp.Data.PushNotifications.PushNotificationHandler;

namespace VacdmApp.Data.PushNotifications
{
    internal partial class PushSender
    {
        private static async Task SendFlowPush(
            FlowMeasure flowMeasure,
            NotificationType notificationType
        )
        {
            var measure = flowMeasure.Measure;
            var endTimeString = flowMeasure.EndTime.ToString("HH:mm") + "Z";

            var notificationMessageString = notificationType switch
            {
                NotificationType.NewFlowMeasure => $"{measure.MeasureTypeString}: {measure.Value}",
                NotificationType.ActiveFlowMeasure
                  => $"{measure.MeasureTypeString}: {measure.Value}, Active until {endTimeString}",
                NotificationType.ExpiredFlowMeasure
                  => $"{measure.MeasureTypeString}: {measure.Value}, Expired at {endTimeString}",
                NotificationType.WithdrawnFlowMeasure
                  => $"{measure.MeasureTypeString}: {measure.Value}, Withdrawn at {flowMeasure.WithdrawnAt?.ToString("HH:mm")}Z",
                _ => throw new ArgumentOutOfRangeException()
            };

            var notificationMessageTitle = notificationType switch
            {
                NotificationType.NewFlowMeasure => $"New Flow Measure {flowMeasure.Ident}",
                NotificationType.ActiveFlowMeasure => $"Active Flow Measure {flowMeasure.Ident}",
                NotificationType.ExpiredFlowMeasure => $"Expired Flow Measure {flowMeasure.Ident}",
                NotificationType.WithdrawnFlowMeasure => $"Withdrawn Flow Measure {flowMeasure.Ident}",
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
            };

            SentNotifications.Add(new(request, flowMeasure.Id));

            await LocalNotificationCenter.Current.Show(request);
        }
    }
}
