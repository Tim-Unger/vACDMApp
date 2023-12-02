using Microsoft.Maui.Controls.Shapes;
using The49.Maui.BottomSheet;
using VACDMApp.Data.Renderer;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Windows.BottomSheets;

public partial class VDGSBottomSheet : BottomSheet
{
    public VDGSBottomSheet()
    {
        InitializeComponent();
    }

    public static string SelectedCallsign = "";

    public static Color Orange = Color.FromArgb("ae7237");

    private static DateTime _tsat;

    private static Label _tobtLabel;

    private static Label _tsatLabel;

    private static Label _timeToGoLabel;

    private void BottomSheet_Loaded(object sender, EventArgs e)
    {
        
        Sender = this;
        SenderPage = VACDMData.SenderPage.Vdgs;
        var vacdmPilot = VACDMPilots.First(x => x.Callsign == SelectedCallsign);

        var callsignLabel = new Label()
        {
            Text = vacdmPilot.Callsign,
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _tobtLabel = new Label()
        {
            Text = $"TOBT {vacdmPilot.Vacdm.Tobt.ToShortTimeString()} UTC",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _tsatLabel = new Label()
        {
            Text = $"TSAT {vacdmPilot.Vacdm.Tsat.ToShortTimeString()} UTC",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _tsat = vacdmPilot.Vacdm.Tsat;
        var timeToGo = Math.Round((DateTime.UtcNow - _tsat).TotalMinutes, 0);
        _timeToGoLabel = new Label()
        {
            Text = timeToGo.ToString(),
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var runwayLabel = new Label()
        {
            Text = $"PLANNED RWY {vacdmPilot.Clearance.DepRwy}",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var sidLabel = new Label()
        {
            Text = $"SID {vacdmPilot.Clearance.Sid}",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        VDGSStackLayout.Children.Add(new Rectangle() { HeightRequest = 200 });
        VDGSStackLayout.Children.Add(callsignLabel);
        VDGSStackLayout.Children.Add(_tobtLabel);
        VDGSStackLayout.Children.Add(_tsatLabel);
        VDGSStackLayout.Children.Add(_timeToGoLabel);
        VDGSStackLayout.Children.Add(runwayLabel);
        VDGSStackLayout.Children.Add(sidLabel);
        VDGSStackLayout.Children.Add(new Rectangle() { HeightRequest = 200 });

        UpdateTimeToGo();
        UpdateDataContinuously();
    }

    private async void UpdateTimeToGo()
    {
        while (true)
        {
            var timeToGoLabel = (Label)VDGSStackLayout.Children[4];

            timeToGoLabel.Text = Math.Round((DateTime.UtcNow - _tsat).TotalMinutes, 0).ToString();

            await Task.Delay(30_000);
        }
    }

    private async Task UpdateDataContinuously()
    {

        //TODO Pause on lost focus/Cancellation Token
        while (true)
        {
            await MainPage.GetAllData();

            var vacdmPilot = VACDMPilots.First(x => x.Callsign == SelectedCallsign);

            _tobtLabel.Text = $"TOBT {vacdmPilot.Vacdm.Tobt.ToShortTimeString()} UTC";
            _tsatLabel.Text = $"TSAT {vacdmPilot.Vacdm.Tsat.ToShortTimeString()} UTC";

            _tsat = vacdmPilot.Vacdm.Tsat;
            var timeToGo = Math.Round((DateTime.UtcNow - _tsat).TotalMinutes, 0);
            _timeToGoLabel.Text = timeToGo.ToString();

            await Task.Delay(60_000);
        }
    }
}
