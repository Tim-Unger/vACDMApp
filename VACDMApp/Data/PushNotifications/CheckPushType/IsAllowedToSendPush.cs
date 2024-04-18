using static VacdmApp.Data.PushNotifications.PushNotificationHandler;

namespace VacdmApp.Data.PushNotifications
{
    internal static partial class PushNotificationHandler
    {
        internal static bool IsAllowedToSendPush(
            NotificationType notificationType,
            bool isOwnFlight
        ) => AllowedToSendPush(notificationType, isOwnFlight);

        internal static bool IsAllowedToSendPush(NotificationType notificationType) =>
            AllowedToSendPush(notificationType, null);

        //If we name this IsAllowedToSendPush as well, we create a recursion, even though it shouldn't
        private static bool AllowedToSendPush(NotificationType notificationType, bool? isOwnFlight)
        {
            if (
                new[]
                {
                    NotificationType.NewFlowMeasure,
                    NotificationType.ActiveFlowMeasure,
                    NotificationType.ActiveFlowMeasure,
                    NotificationType.ExpiredFlowMeasure
                }.Any(x => x == notificationType)
            )
            {
                return CurrentSettings.SendPushFlowMeasures;
            }

            if (isOwnFlight is null)
            {
                throw new ArgumentOutOfRangeException(nameof(notificationType));
            }

            if (isOwnFlight == true)
            {
                return notificationType switch
                {
                    NotificationType.SlotNow => CurrentSettings.SendPushMyFlightInsideWindow,
                    NotificationType.SlotChanged => CurrentSettings.SendPushMyFlightTsatChanged,
                    NotificationType.StartupGiven => CurrentSettings.SendPushMyFlightStartup,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return notificationType switch
            {
                NotificationType.SlotNow => CurrentSettings.SendPushBookmarkFlightInsideWindow,
                NotificationType.SlotChanged => CurrentSettings.SendPushBookmarkTsatChanged,
                NotificationType.StartupGiven => CurrentSettings.SendPushBookmarkStartup,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
