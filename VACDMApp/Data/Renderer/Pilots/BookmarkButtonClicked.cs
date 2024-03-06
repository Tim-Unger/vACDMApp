using CommunityToolkit.Maui.Alerts;
using VacdmApp.Data.PushNotifications;
using VacdmApp.Windows.Views;

namespace VacdmApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static async Task BookmarkButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var parentGrid = (Grid)((Grid)button.Parent).Parent;
            var callsignGrid = (Grid)parentGrid.Children[1];
            var callsignLabel = (Label)callsignGrid.Children[1];
            var callsign = callsignLabel.Text;

            var pilot = Data.VacdmPilots.First(x => x.Callsign == callsign);
            var bookmarks = Data.BookmarkedPilots;

            var image = (Image)((Grid)((Button)sender).Parent).Children[1];

            if (bookmarks.Exists(x => x.Callsign == pilot.Callsign))
            {
                image.Source = "bookmark_outline.svg";

                var removeIndex = bookmarks.FindIndex(x => x.Callsign == pilot.Callsign);

                Data.BookmarkedPilots.RemoveAt(removeIndex);

                var removedToast = Toast.Make(
                    $"Removed Flight {callsign} from your Bookmarks",
                    CommunityToolkit.Maui.Core.ToastDuration.Short,
                    14
                );

                PushNotificationHandler.Unsubscribe(pilot);
                await removedToast.Show();
                //Data.Data.MyFlightView.RenderBookmarks();

                return;
            }

            image.Source = "bookmark.svg";

            Data.BookmarkedPilots.Add(pilot);

            var savedToast = Toast.Make(
                $"Saved Flight {callsign} to your Bookmarks",
                CommunityToolkit.Maui.Core.ToastDuration.Short,
                14
            );

            await savedToast.Show();
            await PushNotificationHandler.SubscribeAsync(pilot);
            //Data.Data.MyFlightView.RenderBookmarks();
        }
    }
}
