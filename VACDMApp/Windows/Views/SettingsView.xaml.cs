using VACDMApp.Data;
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
        if(Settings.Cid is not null)
        {
            CidEntry.Text = Settings.Cid!.ToString();
        }

        DataSources.ForEach(x => DataSourcePicker.Items.Add(x.Name));

        if(Settings.DataSource is not null)
        {
            var selectedSourceIndex = DataSources.FindIndex(x => x.ShortName == Settings.DataSource);
            var dataSources = DataSources;
            var source = Settings.DataSource;
            DataSourcePicker.SelectedIndex = selectedSourceIndex;
        }
    }

    private void ContentView_Unfocused(object sender, FocusEventArgs e)
    {

    }

    private async void DataSourcePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedDataSourceName = DataSourcePicker.SelectedItem.ToString();

        var shortName = DataSources.First(x => x.Name == selectedDataSourceName).ShortName;

        Settings.DataSource = shortName;

        await SettingsData.SetSettingsAsync();

        VACDMData.VACDMData.SetApiUrl();

        await VACDMData.Data.FlightsView.RefreshDataAndView();
    }

}