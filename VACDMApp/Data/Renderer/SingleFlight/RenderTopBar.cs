using Android.Gms.Common.Api.Internal;
using CommunityToolkit.Maui.Alerts;
using VacdmApp.Data.PushNotifications;
using VacdmApp.Data;
using VacdmApp.Windows.BottomSheets;

namespace VacdmApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        internal static Grid TopBarGrid(VacdmPilot pilot)
        {
            var grid = new Grid() { Background = Colors.Transparent };

            var oneStarWidth = new ColumnDefinition(new GridLength(1, GridUnitType.Star));

            grid.ColumnDefinitions.Add(oneStarWidth);
            grid.ColumnDefinitions.Add(oneStarWidth);
            grid.ColumnDefinitions.Add(oneStarWidth);

            var isPilotBookmarked =
                Data.BookmarkedPilots.FirstOrDefault(x => x.Callsign == pilot.Callsign)
                != null;

            var bookmarkImageSource = isPilotBookmarked ? "bookmark.svg" : "bookmark_outline.svg";

            var bookmarkGrid = new Grid() { Background = Colors.Transparent, Padding = new Thickness(40, 20, 20, 30)};

            //var bookmarkImage = new Image() { Source = bookmarkImageSource, Scale = 0.35 ,VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End, Background = Colors.Red };
            var bookmarkImage = new Image() { Source = bookmarkImageSource, Scale = 0.20, VerticalOptions = LayoutOptions.Start };

            var bookmarkButton = new Button()
            {
                Margin = new Thickness(20),
                Background = Colors.Transparent,
            };

            bookmarkGrid.Children.Add(bookmarkImage);
            bookmarkGrid.Children.Add(bookmarkButton);

            var callsignLabel = new Label()
            {
                Text = pilot.Callsign,
                Background = Colors.Transparent,
                TextColor = Colors.White,
                Margin = new Thickness(0),
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var closeButton = new Button()
            {
                Text = "X",
                Margin = new Thickness(0),
                Background = Colors.Transparent,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            bookmarkButton.Clicked += async (sender, e) =>
                await BookmarkButton_Clicked(sender, e, pilot, bookmarkImage);

            closeButton.Clicked += CloseButton_Clicked;

            grid.Children.Add(bookmarkGrid);
            grid.SetColumn(bookmarkGrid, 0);
            //grid.Children.Add(bookmarkImage);
            //grid.SetColumn(bookmarkImage, 0);
            //grid.Children.Add(bookmarkButton);
            //grid.SetColumn(bookmarkButton, 0);

            grid.Children.Add(callsignLabel);
            grid.SetColumn(callsignLabel, 1);

            grid.Children.Add(closeButton);
            grid.SetColumn(closeButton, 2);

            return grid;
        }

        private static async Task BookmarkButton_Clicked(
            object sender,
            EventArgs e,
            VacdmPilot pilot,
            Image image
        )
        {
            var bookmarks = Data.BookmarkedPilots;

            if (bookmarks.Exists(x => x.Callsign == pilot.Callsign))
            {
                image.Source = "bookmark_outline.svg";

                var removeIndex = bookmarks.FindIndex(x => x.Callsign == pilot.Callsign);

                Data.BookmarkedPilots.RemoveAt(removeIndex);

                var removedToast = Toast.Make(
                    $"Removed Flight {pilot.Callsign} from your Bookmarks",
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
                $"Saved Flight {pilot.Callsign} to your Bookmarks",
                CommunityToolkit.Maui.Core.ToastDuration.Short,
                14
            );

            await savedToast.Show();
            await PushNotificationHandler.SubscribeAsync(pilot);
            //Data.Data.MyFlightView.RenderBookmarks();
        }

        private static void CloseButton_Clicked(object sender, EventArgs e)
        {
            ((SingleFlightBottomSheet)Data.Sender).DismissAsync();
        }
    }
}
