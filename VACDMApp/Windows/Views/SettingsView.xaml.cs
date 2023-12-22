using Android.App.Admin;
using VACDMApp.Data;
using VACDMApp.Data.OverridePermissions;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Windows.Views;

public partial class SettingsView : ContentView
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private static bool _isFirstLoad = true;

    private static VACDMData.Settings _settings = new();

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            if (Settings.Cid is not null)
            {
                CidEntry.Text = Settings.Cid!.ToString();
            }

            DataSources.ForEach(x => DataSourcePicker.Items.Add(x.Name));

            if (Settings.DataSource is not null)
            {
                var selectedSourceIndex = DataSources.FindIndex(
                    x => x.ShortName == Settings.DataSource
                );
                var dataSources = DataSources;
                var source = Settings.DataSource;
                DataSourcePicker.SelectedIndex = selectedSourceIndex;
            }

            _settings = Settings;

            await SetToggleStates();

            _isFirstLoad = false;
            return;
        }
    }

    private void ContentView_Unfocused(object sender, FocusEventArgs e) { }

    private async void DataSourcePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedDataSourceName = DataSourcePicker.SelectedItem.ToString();

        var shortName = DataSources.First(x => x.Name == selectedDataSourceName).ShortName;

        Settings.DataSource = shortName;

        await SettingsData.SetSettingsAsync();

        VACDMData.VACDMData.SetApiUrl();

        await VACDMData.Data.FlightsView.RefreshDataAndView();
    }

    private void CidEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CidEntry.Text))
        {
            ValidCidLabel.TextColor = Colors.Red;
            ValidCidLabel.Text = "CID is invalid";
            return;
        }

        var cid = int.Parse(CidEntry.Text);

        if (!cid.IsValidCid())
        {
            ValidCidLabel.TextColor = Colors.Red;
            ValidCidLabel.Text = "CID is invalid";
            return;
        }

        ValidCidLabel.TextColor = Colors.Green;
        ValidCidLabel.Text = "CID is valid";
    }

    private async void EnablePushNotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = EnablePushNotificationsSwitch.IsToggled;

        if (isToggled)
        {
            var grantState = await Permissions.RequestAsync<SendNotifications>();

            if (grantState == PermissionStatus.Granted)
            {
                EnablePushNotificationsSwitch.IsToggled = true;
                return;
            }

            EnablePushNotificationsSwitch.IsToggled = false;
        }

        //Revokation needs to be done within the Notification Handler
        //TODO
    }

    private void MyFlightTsatSwitch_Toggled(object sender, ToggledEventArgs e) { }

    private void MyFlightChangedSwitch_Toggled(object sender, ToggledEventArgs e) { }

    private void MyFlightStartupSwitch_Toggled(object sender, ToggledEventArgs e) { }

    private void BookmarkFlightTsatSwitch_Toggled(object sender, ToggledEventArgs e) { }

    private void BookmarkFlightChangedSwitch_Toggled(object sender, ToggledEventArgs e) { }

    private void BookmarkFlightStartupSwitch_Toggled(object sender, ToggledEventArgs e) { }

    private async Task SetToggleStates()
    {
        var status = await Permissions.CheckStatusAsync<SendNotifications>();

        EnablePushNotificationsSwitch.IsToggled = status == PermissionStatus.Granted;

        if (status == PermissionStatus.Denied)
        {
            MyFlightPushGrid.IsEnabled = false;
            BookmarkedFlightsPushGrid.IsEnabled = false;
        }

        MyFlightTsatSwitch.IsEnabled = true;
        MyFlightChangedSwitch.IsEnabled = true;
        MyFlightStartupSwitch.IsEnabled = true;

        MyFlightTsatSwitch.IsToggled = _settings.SendPushMyFlightInsideWindow;
        MyFlightChangedSwitch.IsToggled = _settings.SendPushMyFlightTsatChanged;
        MyFlightStartupSwitch.IsToggled = _settings.SendPushMyFlightInsideWindow;

        BookmarkFlightTsatSwitch.IsEnabled = true;
        BookmarkFlightChangedSwitch.IsEnabled = true;
        BookmarkFlightStartupSwitch.IsEnabled = true;

        BookmarkFlightTsatSwitch.IsToggled = _settings.SendPushBookmarkFlightInsideWindow;
        BookmarkFlightChangedSwitch.IsToggled = _settings.SendPuishBookmarkTsatChanged;
        BookmarkFlightStartupSwitch.IsToggled = _settings.SendPushBookmarkStartup;
    }
}
