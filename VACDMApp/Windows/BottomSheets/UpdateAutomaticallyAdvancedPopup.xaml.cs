using CommunityToolkit.Maui.Views;

namespace VacdmApp.Windows.Popups;

public partial class UpdateAutomaticallyAdvancedPopup : Popup
{
	public UpdateAutomaticallyAdvancedPopup()
	{
		InitializeComponent();
	}

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        var settings = Data.Data.Settings;

        UpdateIntervalEntry.Text = settings!.UpdateInterval.ToString();

        UpdateVatsimSwitch.IsToggled = settings.UpdateVatsimDataAutomatically;
        UpdateVacdmSwitch.IsToggled = settings.UpdateVacdmDataAutomatically;
        UpdateEcfmpSwitch.IsToggled = settings.UpdateEcfmpDataAutomatically;
    }

    private void UpdateIntervalEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(!int.TryParse(UpdateIntervalEntry.Text, out var interval))
        {
            UpdateIntervalEntry.TextColor = Colors.Red;
        }
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var updateVatsim = UpdateVatsimSwitch.IsToggled;
        Data.Data.Settings.UpdateVatsimDataAutomatically = updateVatsim;
        Preferences.Set("update_vatsim", updateVatsim);

        var updateVacdm = UpdateVacdmSwitch.IsToggled;
        Data.Data.Settings.UpdateVacdmDataAutomatically = updateVacdm;
        Preferences.Set("update_vacdm", updateVacdm);

        var updateEcfmp = UpdateEcfmpSwitch.IsToggled;
        Data.Data.Settings.UpdateEcfmpDataAutomatically = updateEcfmp;
        Preferences.Set("update_ecfmp", updateEcfmp);

        if(!int.TryParse(UpdateIntervalEntry.Text, out var interval))
        {
            UpdateIntervalEntry.TextColor = Colors.Red;
            return;
        }

        Data.Data.Settings.UpdateInterval = interval;
        Preferences.Set("update_interval", interval);

        Close();
    }
}