using Plugin.LocalNotification;
using static VacdmApp.Data.PushNotifications.PushNotificationHandler;

namespace VacdmApp.Data.PushNotifications
{
    internal partial class PushSender
    {
        internal static List<(NotificationRequest request, object CallsignOrId)> SentNotifications =
            new();

        internal static async Task SendPushNotificationAsync(
            VacdmPilot pilot,
            NotificationType notificationType
        ) => await SendFlightPush(pilot, notificationType);

        internal static async Task SendPushNotificationAsync(
            FlowMeasure flowMeasure,
            NotificationType notificationType
        ) => await SendFlowPush(flowMeasure, notificationType);
    }
}
