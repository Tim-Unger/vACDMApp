namespace VACDMApp.Windows.Views;

public partial class LoadingView : ContentView
{
    internal enum LoadingStatus
    {
        Settings,
        VatsimData,
        VacdmData,
        FlowMeasures,
        Airlines,
        Airports
    }

	public LoadingView()
	{
		InitializeComponent();
	}

    //TODO Task.WhenAll() screws up the Text and displays it to quickly
    internal void SetLabelText(LoadingStatus loadingStatus)
    {
        StatusLabel.Text = loadingStatus switch
        {
            LoadingStatus.Settings => "Loading Settings",
            LoadingStatus.VatsimData => "Loading Vatsim Data",
            LoadingStatus.VacdmData => "Loading vACDM Data",
            LoadingStatus.FlowMeasures => "Loading ECFMP Flow Measures",
            LoadingStatus.Airlines => "Loading Airlines",
            LoadingStatus.Airports => "Loading Airports"
        };
    }
}