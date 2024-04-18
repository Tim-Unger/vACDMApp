namespace VacdmApp.Data.PushNotifications
{
    internal static partial class PushNotificationHandler
    {
        internal static void ClearAllSentNotifications(string callsign)
        {
            //TODO implement correctly
            //var pushsFromCallsign = PushSender.SentNotifications.Where(x => x.CallsignOrId == callsign);

            //_ = pushsFromCallsign.Select(
            //    x => LocalNotificationCenter.Current.Clear(x.request.NotificationId)
            //);
        }
    }
}
