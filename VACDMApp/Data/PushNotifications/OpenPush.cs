using Plugin.LocalNotification.EventArgs;
using VacdmApp.Windows.BottomSheets;

namespace VacdmApp.Data.PushNotifications
{
    internal class OpenPush
    {
        internal static void OpenTapped(NotificationEventArgs e)
        {
            var id = e.Request.NotificationId;

            var concernedNotification = PushSender.SentNotifications.FirstOrDefault(
                x => x.request.NotificationId == id
            );

            var callsignOrId = concernedNotification.CallsignOrId;

            if (callsignOrId is null)
            {
                return;
            }

            if(!int.TryParse(callsignOrId.ToString(), out _))
            {
                //TODO 
                return;
            }

            var concernedPilot = Data.VacdmPilots.FirstOrDefault(
                x => x.Callsign == callsignOrId.ToString()
            );

            if (concernedPilot is null)
            {
                return;
            }

            var singleFlightSheet = new SingleFlightBottomSheet();

            singleFlightSheet.ShowAsync();
        }
    }
}
