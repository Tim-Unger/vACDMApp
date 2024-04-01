using Plugin.LocalNotification;

namespace VacdmApp.Data.PushNotifications
{
    internal class ClearPush
    {
        internal static void ClearAll(string callsign)
        {
            return;
            //TOD implement correctly
            var pushsFromCallsign = PushSender.SentNotifications.Where(x => x.CallsignOrId == callsign);

            _ = pushsFromCallsign.Select(
                x => LocalNotificationCenter.Current.Clear(x.request.NotificationId)
            );
        }
    }
}
