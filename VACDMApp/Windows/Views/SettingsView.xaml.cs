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
            DataSourcePicker.SelectedIndex = selectedSourceIndex;
        }
    }

    private void ContentView_Unfocused(object sender, FocusEventArgs e)
    {

    }

    private void DataSourcePicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
}