namespace VACDMApp.Windows.Views;

using Microsoft.Maui.Controls;
using VACDMApp.VACDMData;
using VACDMApp.Windows.BottomSheets;

public partial class MyFlightView : ContentView
{
    private static bool _isFirstLoad = true;

    public MyFlightView()
    {
        InitializeComponent();
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

        var page = Shell.Current.CurrentPage;

        var cidText = CidText.Text;

        if (!int.TryParse(cidText, out var cid))
        {
            //Should not be possible since we only allow numeric input, but just in case
            await page.DisplayAlert("Invalid CID", "The provided CID was not a number", "OK");
        }

        if (!IsValidCid(cid))
        {
            await page.DisplayAlert("Invalid CID", "The provided CID does not exist", "OK");
        }

        var pilot = Data.VatsimPilots.FirstOrDefault(x => x.cid == cid);

        if (pilot is null)
        {
            await page.DisplayAlert(
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
            await page.DisplayAlert("No vACDM Times", "There are no vACDM Times available for your flight", "OK");

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

    private void ShowVdgsButton_Clicked(object sender, EventArgs e)
    {
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

            TimeLabel.Text = $"{now.ToString("T")}Z";

            await Task.Delay(200);
        }
    }
}
