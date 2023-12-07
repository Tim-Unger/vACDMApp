using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Windows.Views;

public partial class SettingsView : ContentView
{
	public SettingsView()
	{
		InitializeComponent();
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        DataSources.ForEach(x => DataSourcePicker.Items.Add(x.Name));
    }

    private void ContentView_Unfocused(object sender, FocusEventArgs e)
    {

    }

    private void DataSourcePicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
}