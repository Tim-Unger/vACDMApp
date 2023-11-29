using Microsoft.Maui.Controls.Shapes;
using The49.Maui.BottomSheet;
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

    private void BottomSheet_Loaded(object sender, EventArgs e)
    {

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

        var tobtLabel = new Label()
        {
            Text = $"TOBT {vacdmPilot.Vacdm.Tobt.ToShortTimeString()} UTC",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var tsatLabel = new Label()
        {
            Text = $"TSAT {vacdmPilot.Vacdm.Tsat.ToShortTimeString()} UTC",
            TextColor = Orange,
            FontFamily = "AdvancedDot",
            FontAttributes = FontAttributes.Bold,
            FontSize = 30,
            HorizontalTextAlignment = TextAlignment.Center
        };

        var timeToGo = Math.Round((vacdmPilot.Vacdm.Tsat - DateTime.UtcNow).TotalMinutes, 0);
        var timeToGoLabel = new Label()
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
        VDGSStackLayout.Children.Add(tobtLabel);
        VDGSStackLayout.Children.Add(tsatLabel);
        VDGSStackLayout.Children.Add(timeToGoLabel);
        VDGSStackLayout.Children.Add(runwayLabel);
        VDGSStackLayout.Children.Add(sidLabel);
        VDGSStackLayout.Children.Add(new Rectangle() { HeightRequest = 200 });
    }
}
