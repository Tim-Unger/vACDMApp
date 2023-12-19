namespace VACDMApp.Windows.Views;

using Microsoft.Maui.Controls;
using VACDMApp.VACDMData;
using VACDMApp.Data.Renderer;
using VACDMApp.Windows.BottomSheets;
using Java.Util.Zip;
using VACDMApp.Data;

public partial class MyFlightView : ContentView
{
    private static bool _isFirstLoad = true;

    private static Page _page;

    public MyFlightView()
    {
        InitializeComponent();
        _page = Shell.Current.CurrentPage;
    }

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            await GetCurrentTime();
        }

        RenderBookmarks();

        _isFirstLoad = false;
    }

    private async void FindCidButton_Clicked(object sender, EventArgs e)
    {
        if (Data.Settings.Cid is not null && string.IsNullOrWhiteSpace(CidText.Text))
        {
            CidText.Text = Data.Settings.Cid.ToString();
        }

        var cidText = CidText.Text;

        if (cidText is null)
        {
            await _page.DisplayAlert("No CID", "Please enter a CID", "Ok");
            return;
        }

        if (!int.TryParse(cidText, out var cid))
        {
            //Should not be possible since we only allow numeric input, but just in case
            await _page.DisplayAlert("Invalid CID", "The provided CID was not a number", "OK");
            return;
        }

        if (!cid.IsValidCid())
        {
            await _page.DisplayAlert("Invalid CID", "The provided CID does not exist", "OK");
            return;
        }

        var pilot = Data.VatsimPilots.Find(x => x.cid == cid);

        if (pilot is null)
        {
            await _page.DisplayAlert(
                "CID not found",
                "Looks like you don't have an active flight at the moment",
                "OK"
            );

            return;
        }

        var vacdmPilot = Data.VACDMPilots.Find(
            x => x.Callsign.Equals(pilot.callsign, StringComparison.InvariantCultureIgnoreCase)
        );

        if (vacdmPilot is null)
        {
            await _page.DisplayAlert(
                "No vACDM Times",
                "There are no vACDM Times available for your flight",
                "OK"
            );

            return;
        }

        ShowVdgsButton.IsEnabled = true;
        FindCidButton.IsEnabled = true;

        //Trick to hide the onscreen keyboard
        CidText.IsEnabled = false;
        CidText.IsEnabled = true;

        var singleFlightSheet = new SingleFlightBottomSheet();

        SingleFlightBottomSheet.SelectedCallsign = vacdmPilot.Callsign;
        
        await singleFlightSheet.ShowAsync();
    }

    private async void ShowVdgsButton_Clicked(object sender, EventArgs e)
    {
        var cidText = CidText.Text;

        if (cidText is null)
        {
            await _page.DisplayAlert("No CID", "Please enter a CID", "Ok");
            return;
        }

        var cid = int.Parse(CidText.Text);
        var pilot = Data.VatsimPilots.First(x => x.cid == cid);

        //Trick to hide the onscreen keyboard
        CidText.IsEnabled = false;
        CidText.IsEnabled = true;

        VDGSBottomSheet.SelectedCallsign = pilot.callsign;
        var vdgsSheet = new VDGSBottomSheet();

        vdgsSheet.ShowAsync();
    }

    private async Task GetCurrentTime()
    {
        while (true)
        {
            var now = DateTime.UtcNow;

            TimeLabel.Text = $"{now:T}Z";

            await Task.Delay(200);
        }
    }

    private void ContentView_Focused(object sender, FocusEventArgs e)
    {
        //TODO better loading/unloading
        RenderBookmarks();
    }

    internal void RenderBookmarks()
    {
        //BookmarksStackLayout.Children.Clear();
        var bookmarks = Bookmarks.Render(Data.BookmarkedPilots);
        bookmarks.ForEach(BookmarksStackLayout.Children.Add);
        BookmarksScrollView.Content = BookmarksStackLayout;
    }
}
