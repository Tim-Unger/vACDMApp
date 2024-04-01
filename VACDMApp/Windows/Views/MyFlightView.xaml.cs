namespace VacdmApp.Windows.Views;

using Microsoft.Maui.Controls;
using VacdmApp.Data;
using VacdmApp.Data.Renderer;
using VacdmApp.Windows.BottomSheets;
using VacdmApp.Data;

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
        //RenderBookmarks();
        RenderOwnFlightView();

        if (_isFirstLoad)
        {
            await GetCurrentTime();

            _isFirstLoad = false;
            return;
        }

        _isFirstLoad = false;
    }

    private async void FindCidButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchText.Text) && string.IsNullOrWhiteSpace(Data.Settings.Cid.ToString()))
        {
            return;
        }

        var isCid = int.TryParse(SearchText.Text, out _);
        
        var pilot = isCid ? await GetVatsimPilotByCid() : await GetVatsimPilotByCallsign();

        if(pilot is null)
        {
            await _page.DisplayAlert(
                "CID or Callsign not found",
                "Looks like you don't have an active flight at the moment",
                "OK"
            );

            return;
        }

        var vacdmPilot = Data.VacdmPilots.Find(
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

        //Trick to hide the onscreen keyboard
        SearchText.IsEnabled = false;
        SearchText.IsEnabled = true;

        SingleFlightBottomSheet.SelectedCallsign = vacdmPilot.Callsign;

        var singleFlightSheet = new SingleFlightBottomSheet();

        await singleFlightSheet.ShowAsync();
    }

    private async void ShowVdgsButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchText.Text) && string.IsNullOrWhiteSpace(Data.Settings.Cid.ToString()))
        {
            return;
        }

        var isCid = int.TryParse(SearchText.Text, out _);

        var pilot = isCid ? await GetVatsimPilotByCid() : await GetVatsimPilotByCallsign();

        if (pilot is null)
        {
            await _page.DisplayAlert(
                "CID or Callsign not found",
                "Looks like you don't have an active flight at the moment",
                "OK"
            );

            return;
        }

        var vacdmPilot = Data.VacdmPilots.Find(
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

        //Trick to hide the onscreen keyboard
        SearchText.IsEnabled = false;
        SearchText.IsEnabled = true;

        VDGSBottomSheet.SelectedCallsign = pilot.callsign;
        var vdgsSheet = new VDGSBottomSheet();

        vdgsSheet.ShowAsync();
    }

    private async Task GetCurrentTime()
    {
        while (true)
        {
            var now = DateTime.UtcNow;

            TimeLabel.Text = $"{now:T}" + "Z";

            await Task.Delay(200);
        }
    }

    internal void RenderBookmarks()
    {
        //TODO only works once then breaks somehow
        BookmarksStackLayout.Children.Clear();

        if(Data.BookmarkedPilots.Count == 0)
        {
            return;
        }

        var bookmarks = Bookmarks.Render(Data.BookmarkedPilots);
        bookmarks.ForEach(BookmarksStackLayout.Children.Add);

        //BookmarksScrollView.Content = BookmarksStackLayout;
    }
   
    private void RenderOwnFlightView()
    {
        OwnFlightGrid.Children.Clear();

        var vatsimPilots = Data.VatsimPilots;
        var vacdmPilots = Data.VacdmPilots;
        var cid = Data.Settings.Cid;
        
        if(vatsimPilots.Find(x => x.cid == cid) is null)
        {
            OwnFlightGrid.Children.Add(NoFlightLabel);
            return;
        }

        var callsign = vatsimPilots.First(x => x.cid == cid).callsign;

        if(vacdmPilots.Find(x => x.Callsign == callsign) is null)
        {
            OwnFlightGrid.Children.Add(NoFlightLabel);
            return;
        }

        var yourFlightLabel = new Label()
        {
            Text = "Your Flight",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Start,
            TextColor = Colors.Grey,
            FontSize = 18,
            Margin = 10
        };

        var pilot = vacdmPilots.Find(x => x.Callsign == callsign);
        var flight = Bookmarks.RenderBookmark(pilot);
        
        flight.VerticalOptions = LayoutOptions.Start;

        var stackLayout = new VerticalStackLayout();

        stackLayout.Children.Add(yourFlightLabel);
        stackLayout.Children.Add(flight);

        OwnFlightGrid.Children.Add(stackLayout);
    }

    private static readonly Label NoFlightLabel = new()
    {
        Text = "You dont't have an active vACDM Flight at the moment.\r\nYou can look up another CID at the top",
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center,
        TextColor = Colors.Grey,
        HorizontalTextAlignment = TextAlignment.Center,
        FontSize = 18,
    };

    private async Task<Pilot?> GetVatsimPilotByCid()
    {
        if (Data.Settings.Cid is not null && string.IsNullOrWhiteSpace(SearchText.Text))
        {
            SearchText.Text = Data.Settings.Cid.ToString();
        }

        var searchText = SearchText.Text;

        if (SearchText is null)
        {
            await _page.DisplayAlert("No CID", "Please enter a CID or Callsign", "Ok");
            return null;
        }

        if (!int.TryParse(searchText, out var cid))
        {
            await _page.DisplayAlert("Invalid CID", "The provided CID was not a number", "OK");
            return null;
        }

        if (!cid.IsValidCid())
        {
            await _page.DisplayAlert("Invalid CID", "The provided CID does not exist", "OK");
            return null;
        }

        return Data.VatsimPilots.Find(x => x.cid == cid);
    }

    private async Task<Pilot?> GetVatsimPilotByCallsign()
    {
        if (Data.Settings.Cid is not null && string.IsNullOrWhiteSpace(SearchText.Text))
        {
            SearchText.Text = Data.Settings.Cid.ToString();
        }

        var callsign = SearchText.Text.ToUpperInvariant();

        if (callsign is null)
        {
            await _page.DisplayAlert("No CID", "Please enter a CID or Callsign", "Ok");
            return null;
        }

        var vatsimPilot = Data.VatsimPilots.FirstOrDefault(x => x.callsign == callsign);

        return vatsimPilot is null ? null : vatsimPilot;
    }
}
