using Microsoft.Maui.Controls.Shapes;
using The49.Maui.BottomSheet;
using VacdmApp.Data.Renderer;
using static VacdmApp.Data.Data;

namespace VacdmApp.Windows.BottomSheets;

public partial class VDGSBottomSheet : BottomSheet
{
    public VDGSBottomSheet()
    {
        InitializeComponent();
    }

    public static string SelectedCallsign = "";

    public readonly Color Orange = Color.FromArgb("ae7237");

    private DateTime _tsat;

    private Label _tobtLabel;

    private Label _tsatLabel;

    private Label _timeToGoLabel;

    private static CancellationTokenSource _cancellationTokenSource = new();

    private async void BottomSheet_Loaded(object sender, EventArgs e)
    {
        Sender = this;
        SenderPage = Data.SenderPage.Vdgs;
        var vacdmPilot = VacdmPilots.First(x => x.Callsign == SelectedCallsign);

        var callsignLabel = new Label()
        {
            Text = vacdmPilot.Callsign,
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.None,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _tobtLabel = new Label()
        {
            Text = $"TOBT {vacdmPilot.Vacdm.Tobt.ToShortTimeString()} UTC",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.None,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _tsatLabel = new Label()
        {
            Text = $"TSAT {vacdmPilot.Vacdm.Tsat.ToShortTimeString()} UTC",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.None,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _tsat = vacdmPilot.Vacdm.Tsat;
        var timeToGo = Math.Round((DateTime.UtcNow - _tsat).TotalMinutes, 0);

        var timeToGoString = timeToGo.ToString();

        if(timeToGo > 60 || timeToGo < -60)
        {
            timeToGoString = ">60";
        }

        _timeToGoLabel = new Label()
        {
            Text = timeToGoString,
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.None,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var runwayLabel = new Label()
        {
            Text = $"PLANNED RWY {vacdmPilot.Clearance.DepRwy}",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.None,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var sidLabel = new Label()
        {
            Text = $"SID {vacdmPilot.Clearance.Sid}",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.None,
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

        await UpdateTimeToGo();
        await UpdateDataContinuously();
    }

    private async Task UpdateTimeToGo()
    {
        while (true)
        {
            var timeToGoLabel = (Label)VDGSStackLayout.Children[4];
            
            var timeToGo = Math.Round((DateTime.UtcNow - _tsat).TotalMinutes, 0);

            var timeToGoString = timeToGo.ToString();

            if (timeToGo > 60 || timeToGo < -60)
            {
                timeToGoString = "> 60";
            }

            timeToGoLabel.Text = timeToGoString;

            await Task.Delay(TimeSpan.FromSeconds(30));
        }
    }

    private async Task UpdateDataContinuously()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            var vacdmPilot = VacdmPilots.First(x => x.Callsign == SelectedCallsign);

            _tobtLabel.Text = $"TOBT {vacdmPilot.Vacdm.Tobt.ToShortTimeString()} UTC";
            _tsatLabel.Text = $"TSAT {vacdmPilot.Vacdm.Tsat.ToShortTimeString()} UTC";

            _tsat = vacdmPilot.Vacdm.Tsat;
            var timeToGo = Math.Round((DateTime.UtcNow - _tsat).TotalMinutes, 0);
            _timeToGoLabel.Text = timeToGo.ToString();

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    private void BottomSheet_Unloaded(object sender, EventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}
