namespace VacdmApp.Data.PushNotifications 
{
    internal partial class PushNotificationHandler
    {
        internal static bool IsAllowedToSendPush(NotificationType notificationType, bool isOwnFlight)
        {
            if (isOwnFlight)
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
