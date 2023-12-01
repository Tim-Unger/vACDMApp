namespace VACDMApp.Windows.Views;

using Microsoft.Maui.Controls;
using VACDMApp.VACDMData;
using VACDMApp.VACDMData.Renderer;
using VACDMApp.Windows.BottomSheets;

public partial class MyFlightView : ContentView
{
    private static bool _isFirstLoad = true;

    private static Page _page;

    public MyFlightView()
    {
        InitializeComponent();
        _page = Shell.Current.CurrentPage;
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad) 
        {
            GetCurrentTime();
        }

        _isFirstLoad = false;
    }

    private async void FindCidButton_Clicked(object sender, EventArgs e)
    {
        if(Data.Settings.Cid is not null)
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

        if (!IsValidCid(cid))
        {
            await _page.DisplayAlert("Invalid CID", "The provided CID does not exist", "OK");
            return;
        }

        var pilot = Data.VatsimPilots.FirstOrDefault(x => x.cid == cid);

        if (pilot is null)
        {
            await _page.DisplayAlert(
                "CID not found",
                "Looks like you don't have an active flight at the moment",
                "OK"
            );

            return;
        }

        var vacdmPilot = Data.VACDMPilots.FirstOrDefault(
            x => x.Callsign.Equals(pilot.callsign, StringComparison.InvariantCultureIgnoreCase)
        );

        if (vacdmPilot is null)
        {
            await _page.DisplayAlert("No vACDM Times", "There are no vACDM Times available for your flight", "OK");

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

        if(cidText is null)
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

    private static bool IsValidCid(int cid)
    {
        //CID cannot be smaller than 800_000 (lowest founder CID is 800_006)
        if (cid < 800000)
        {
            return false;
        }

        //CID is an older CID and therefore has one less digit (determined by the first letter)
        if (new[] { 8, 9 }.Any(x => x == int.Parse(cid.ToString()[0].ToString())))
        {
            if (cid.ToString().Length != 6)
            {
                return false;
            }

            return true;
        }

        //Newer CIDs must be 7 letters long (for now)
        if (cid.ToString().Length != 7)
        {
            return false;
        }

        return true;
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
}
