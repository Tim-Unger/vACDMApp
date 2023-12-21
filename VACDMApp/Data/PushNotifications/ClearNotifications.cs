using Plugin.LocalNotification;

namespace VACDMApp.Data.PushNotifications
{
    internal class ClearPush
    {
        internal static void ClearAll(string callsign)
        {
            //TOD implement correctly
            var pushsFromCallsign = PushSender.SentNotifications.Where(x => x.callsign == callsign);

            _ = pushsFromCallsign.Select(x => LocalNotificationCenter.Current.Clear(x.request.NotificationId));
        }

    }
}
