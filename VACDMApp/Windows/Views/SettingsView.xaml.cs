using Android.App.Admin;
using VACDMApp.Data;
using VACDMApp.Data.OverridePermissions;
using VACDMApp.VACDMData;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Windows.Views;

public partial class SettingsView : ContentView
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private static bool _isFirstLoad = true;

    private VACDMData.Settings _settings = new();

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            if (VACDMData.Data.Settings.Cid is not null)
            {
                CidEntry.Text = VACDMData.Data.Settings.Cid!.ToString();
            }

            DataSources.ForEach(x => DataSourcePicker.Items.Add(x.Name));

            if (VACDMData.Data.Settings.DataSource is not null)
            {
                var selectedSourceIndex = DataSources.FindIndex(
                    x => x.ShortName == VACDMData.Data.Settings.DataSource
                );
                var dataSources = DataSources;
                var source = VACDMData.Data.Settings.DataSource;
                DataSourcePicker.SelectedIndex = selectedSourceIndex;
            }

            _settings = VACDMData.Data.Settings;

            await SetToggleStates();

            _isFirstLoad = false;
            return;
        }
    }

    private async void DataSourcePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedDataSourceName = DataSourcePicker.SelectedItem.ToString();

        var shortName = DataSources.First(x => x.Name == selectedDataSourceName).ShortName;

        VACDMData.Data.Settings.DataSource = shortName;

        var pilotTask = await VACDMPilotsData.GetVACDMPilotsAsync();

        VACDMPilots = pilotTask;

        VACDMData.VACDMData.SetApiUrl();

        var settings = VACDMData.Data.Settings;

        Preferences.Default.Set("data_source", settings.DataSource);

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

        Preferences.Set("cid", cid);
    }

    private async void EnablePushNotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = EnablePushNotificationsSwitch.IsToggled;

        if (!isToggled)
        {
            //Revokation needs to be done within the Notification Handler
            //TODO
            AllowPushBookmarkGrid.IsVisible = true;
            AllowPushMyFlightGrid.IsVisible = true;
            Preferences.Set("allow_push", false);
            return;
        }

        var grantState = await Permissions.RequestAsync<SendNotifications>();

        if (grantState != PermissionStatus.Granted)
        {
            EnablePushNotificationsSwitch.IsToggled = false;
            AllowPushBookmarkGrid.IsVisible = true;
            AllowPushMyFlightGrid.IsVisible = true;
            Preferences.Set("allow_push", false);

            return;
        }

        EnablePushNotificationsSwitch.IsToggled = true;
        VACDMData.Data.Settings.AllowPushNotifications = true;
        AllowPushBookmarkGrid.IsVisible = false;
        AllowPushMyFlightGrid.IsVisible = false;
        Preferences.Set("allow_push", true);
    }

    private void MyFlightTsatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = ((Switch)sender).IsToggled;

        VACDMData.Data.Settings.SendPushMyFlightTsatChanged = isToggled;
        Preferences.Set("push_my_flight_window", isToggled);
    }

    private void MyFlightChangedSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = ((Switch)sender).IsToggled;

        VACDMData.Data.Settings.SendPushMyFlightTsatChanged = isToggled;
        Preferences.Set("push_my_flight_tsat", isToggled);
    }

    private void MyFlightStartupSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = ((Switch)sender).IsToggled;

        VACDMData.Data.Settings.SendPushMyFlightInsideWindow = isToggled;
        Preferences.Set("push_my_flight_startup", isToggled);
    }

    private void BookmarkFlightTsatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = ((Switch)sender).IsToggled;

        VACDMData.Data.Settings.SendPushBookmarkFlightInsideWindow = isToggled;
        Preferences.Set("push_bookmark_window", isToggled);
    }

    private void BookmarkFlightChangedSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = ((Switch)sender).IsToggled;

        VACDMData.Data.Settings.SendPushBookmarkTsatChanged = isToggled;
        Preferences.Set("push_bookmark_tsat", isToggled);
    }

    private void BookmarkFlightStartupSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var isToggled = ((Switch)sender).IsToggled;

        VACDMData.Data.Settings.SendPushBookmarkStartup = isToggled;
        Preferences.Set("push_bookmark_startup", isToggled);
    }

    private async Task SetToggleStates()
    {
        var status = await Permissions.CheckStatusAsync<SendNotifications>();

        EnablePushNotificationsSwitch.IsToggled = status == PermissionStatus.Granted;

        if (status == PermissionStatus.Denied)
        {
            AllowPushBookmarkGrid.IsVisible = true;
            AllowPushMyFlightGrid.IsVisible = true;
            
            MyFlightPushGrid.IsEnabled = false;
            BookmarkedFlightsPushGrid.IsEnabled = false;

            return;
        }

        var isPushAllowed = Preferences.Get("allow_push", false);

        AllowPushBookmarkGrid.IsVisible = !isPushAllowed;
        AllowPushMyFlightGrid.IsVisible = !isPushAllowed;

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
        BookmarkFlightChangedSwitch.IsToggled = _settings.SendPushBookmarkTsatChanged;
        BookmarkFlightStartupSwitch.IsToggled = _settings.SendPushBookmarkStartup;
    }
}
