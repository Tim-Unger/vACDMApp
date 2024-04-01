using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Net.Http.Json;
using VacdmApp.Data;
using VacdmApp.Data.OverridePermissions;
using VacdmApp.Windows.BottomSheets;
using static VacdmApp.Data.Data;

namespace VacdmApp.Windows.Views
{
    internal class Rating
    {
        public string id { get; set; }
        public int rating { get; set; }
        public int pilotrating { get; set; }
        public int militaryrating { get; set; }
        public object susp_date { get; set; }
        public DateTime reg_date { get; set; }
        public string region { get; set; }
        public string division { get; set; }
        public string subdivision { get; set; }
        public DateTime lastratingchange { get; set; }
    }

    public partial class SettingsView : ContentView
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private bool _isFirstLoad = true;

        private Settings _settings = new();

        private bool _pushFirModal = false;

        private CancellationTokenSource _cancellationTokenSource = new();

        private async void ContentView_Loaded(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                if (Data.Data.Settings.Cid is not null)
                {
                    CidEntry.Text = Data.Data.Settings.Cid!.ToString();
                }

                DataSources.ForEach(x => DataSourcePicker.Items.Add(x.Name));

                if (Data.Data.Settings.DataSource is not null)
                {
                    var selectedSourceIndex = DataSources.FindIndex(
                        x => x.ShortName == Data.Data.Settings.DataSource
                    );
                    var dataSources = DataSources;
                    var source = Data.Data.Settings.DataSource;
                    DataSourcePicker.SelectedIndex = selectedSourceIndex;
                }

                _settings = Data.Data.Settings;

                await SetToggleStates();

                _isFirstLoad = false;
                return;
            }

            _isFirstLoad = false;
        }

        private async void DataSourcePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedDataSourceName = DataSourcePicker.SelectedItem.ToString();

            var shortName = DataSources.First(x => x.Name == selectedDataSourceName).ShortName;

            Data.Data.Settings.DataSource = shortName;

            var pilotTask = await VACDMPilotsData.GetVACDMPilotsAsync();

            VacdmPilots = pilotTask;

           Data.VacdmData.SetApiUrl();

            var settings = Data.Data.Settings;

            Preferences.Default.Set("data_source", settings.DataSource);

            //FlowMeasureFirs.ForEach(x => FlowMesureFirsCollectionView.AddLogicalChild(new Label() { Text = x.Name }));

            await Data.Data.FlightsView.RefreshDataAndView();
        }

        private async void CidEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _cancellationTokenSource.Cancel();

            //Reset the token once it has been fired
            _cancellationTokenSource = new CancellationTokenSource();

            if (string.IsNullOrWhiteSpace(CidEntry.Text))
            {
                ValidCidLabel.TextColor = Colors.Red;
                ValidCidLabel.Text = "CID is invalid";
                CidData.Text = "Invalid";
                return;
            }

            var cid = int.Parse(CidEntry.Text);

            if (!cid.IsValidCid())
            {
                ValidCidLabel.TextColor = Colors.Red;
                ValidCidLabel.Text = "CID is invalid";
                CidData.Text = "Invalid";
                return;
            }

            ValidCidLabel.TextColor = Colors.Green;
            ValidCidLabel.Text = "CID is valid";

            var ratingUrl = $"https://api.vatsim.net/api/ratings/{cid}";

            CidData.Text = "Loading";

            try
            {
                var cidData = await VacdmData.Client.GetFromJsonAsync<Rating>(
                    ratingUrl,
                    _cancellationTokenSource.Token
                );

                Preferences.Set("cid", cid);

                var rating = GetRatingString(cidData.rating);

                CidData.Text = rating;

                var subdivision = cidData.subdivision;
            }
            catch
            {
                ValidCidLabel.TextColor = Colors.Red;
                ValidCidLabel.Text = "CID is invalid";
            }
        }

        private async void EnablePushNotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = EnablePushNotificationsSwitch.IsToggled;

            if (!isToggled)
            {
                //Revokation needs to be done within the Notification Handler
                ////TODO
                //AllowPushBookmarkGrid.IsVisible = true;
                //AllowPushMyFlightGrid.IsVisible = true;
                //AllowPushFlowRect.IsVisible = true;
                Data.Data.Settings.AllowPushNotifications = false;
                PushSettingsGrid.IsVisible = false;
                Preferences.Set("allow_push", false);
                return;
            }

            var grantState = await Permissions.RequestAsync<SendNotifications>();

            if (grantState != PermissionStatus.Granted)
            {
                EnablePushNotificationsSwitch.IsToggled = false;
                //AllowPushBookmarkGrid.IsVisible = true;
                //AllowPushMyFlightGrid.IsVisible = true;
                //AllowPushFlowRect.IsVisible = true;
                PushSettingsGrid.IsVisible = false;
                Data.Data.Settings.AllowPushNotifications = false;
                Preferences.Set("allow_push", false);
                return;
            }

            PushSettingsGrid.IsVisible = true;
            EnablePushNotificationsSwitch.IsToggled = true;
            Data.Data.Settings.AllowPushNotifications = true;
            AllowPushBookmarkGrid.IsVisible = false;
            AllowPushMyFlightGrid.IsVisible = false;
            AllowPushFlowRect.IsVisible = false;
            Preferences.Set("allow_push", true);
        }

        private void MyFlightTsatSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushMyFlightTsatChanged = isToggled;
            Preferences.Set("push_my_flight_window", isToggled);
        }

        private void MyFlightChangedSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushMyFlightTsatChanged = isToggled;
            Preferences.Set("push_my_flight_tsat", isToggled);
        }

        private void MyFlightStartupSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushMyFlightInsideWindow = isToggled;
            Preferences.Set("push_my_flight_startup", isToggled);
        }

        private void BookmarkFlightTsatSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushBookmarkFlightInsideWindow = isToggled;
            Preferences.Set("push_bookmark_window", isToggled);
        }

        private void BookmarkFlightChangedSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushBookmarkTsatChanged = isToggled;
            Preferences.Set("push_bookmark_tsat", isToggled);
        }

        private void BookmarkFlightStartupSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushBookmarkStartup = isToggled;
            Preferences.Set("push_bookmark_startup", isToggled);
        }

        private async void UpdateAutomaticallySwitch_Toggled(object sender, ToggledEventArgs e)
        {
            UpdateAutomaticallyActivityIndicator.IsVisible = true;
            UpdateAutomaticallySwitch.IsVisible = false;

            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.UpdateAutomatically = isToggled;

            Preferences.Set("update_automatically", isToggled);

            if (!isToggled)
            {
                DataHandler.Cancel();

                UpdateAutomaticallyActivityIndicator.IsVisible = false;
                UpdateAutomaticallySwitch.IsVisible = true;
                UpdateAutomaticallySwitch.IsEnabled = true;
                return;
            }

            UpdateAutomaticallyActivityIndicator.IsVisible = false;
            UpdateAutomaticallySwitch.IsVisible = true;

            await DataHandler.ResumeAsync();
        }


        private async Task SetToggleStates()
        {
            var status = await Permissions.CheckStatusAsync<SendNotifications>();

            var isPushAllowed = Preferences.Get("allow_push", false);

            if(status != PermissionStatus.Granted)
            {
                isPushAllowed = false;
            }

            EnablePushNotificationsSwitch.IsToggled = isPushAllowed;

            if (!isPushAllowed)
            {
                AllowPushBookmarkGrid.IsVisible = true;
                AllowPushMyFlightGrid.IsVisible = true;
                AllowPushFlowRect.IsVisible = true;

                MyFlightPushGrid.IsEnabled = false;
                BookmarkedFlightsPushGrid.IsEnabled = false;

                return;
            }

            AllowPushBookmarkGrid.IsVisible = false;
            AllowPushMyFlightGrid.IsVisible = false;
            AllowPushFlowRect.IsVisible = false;

            MyFlightTsatSwitch.IsEnabled = true;
            MyFlightChangedSwitch.IsEnabled = true;
            MyFlightStartupSwitch.IsEnabled = true;
            MyFlightSlotUnconfirmedSwitch.IsEnabled = true;

            MyFlightTsatSwitch.IsToggled = _settings.SendPushMyFlightInsideWindow;
            MyFlightChangedSwitch.IsToggled = _settings.SendPushMyFlightTsatChanged;
            MyFlightStartupSwitch.IsToggled = _settings.SendPushMyFlightInsideWindow;
            MyFlightSlotUnconfirmedSwitch.IsToggled = _settings.SendPushMyFlightSlotUnconfirmed;

            BookmarkFlightTsatSwitch.IsEnabled = true;
            BookmarkFlightChangedSwitch.IsEnabled = true;
            BookmarkFlightStartupSwitch.IsEnabled = true;

            BookmarkFlightTsatSwitch.IsToggled = _settings.SendPushBookmarkFlightInsideWindow;
            BookmarkFlightChangedSwitch.IsToggled = _settings.SendPushBookmarkTsatChanged;
            BookmarkFlightStartupSwitch.IsToggled = _settings.SendPushBookmarkStartup;

            FlowMeasuresSwitch.IsToggled = _settings.SendPushFlowMeasures;

            if (_settings.SendPushFlowMeasures)
            {
                EditFlowFirsButton.IsVisible = true;
            }

            UpdateAutomaticallySwitch.IsToggled = _settings.UpdateAutomatically;
        }

        private void MyFlightSlotUnconfirmedSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushBookmarkStartup = isToggled;

            Preferences.Set("push_my_flight_slot_unconfirmed", isToggled);
        }

        private async void FlowMeasuresSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var isToggled = ((Switch)sender).IsToggled;

            Data.Data.Settings.SendPushFlowMeasures = isToggled;

            EditFlowFirsButton.IsVisible = isToggled;

            Preferences.Set("push_flow_measures", isToggled);

            if (!isToggled)
            {
                return;
            }

            if (!_pushFirModal)
            {
                _pushFirModal = !_pushFirModal;
                return;
            }

            _pushFirModal = !_pushFirModal;

            //FlowMeasuresSwitch.IsVisible = false;
            //FlowMeasuresBusyIndicator.IsVisible = true;

            if (!string.IsNullOrWhiteSpace(Preferences.Get("flow_measure_push_firs", "")))
            {
                _cancellationTokenSource.Cancel();
                var firBottomSheet = new FirBottomSheet();

                firBottomSheet.Closed += FirBottomSheet_Closed;

                await AppShell.Current.CurrentPage.ShowPopupAsync(firBottomSheet);
            }
        }

        private void FirBottomSheet_Closed(object sender, PopupClosedEventArgs e)
        {
            //FlowMeasuresSwitch.IsVisible = true;
            //FlowMeasuresBusyIndicator.IsVisible = false;
        }

        private string GetRatingString(int index) =>
            index switch
            {
                -1 => "INA",
                0 => "SUS",
                1 => "OBS",
                2 => "S1",
                3 => "S2",
                4 => "S3",
                5 => "C1",
                6 => "C2",
                7 => "C3",
                8 => "I1",
                9 => "I2",
                10 => "I3",
                11 => "SUP",
                12 => "ADM"
            };

        private async void EditFlowFirsButton_Clicked(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();

            var firBottomSheet = new FirBottomSheet();

            firBottomSheet.Closed += FirBottomSheet_Closed;

            await AppShell.Current.CurrentPage.ShowPopupAsync(firBottomSheet);
        }
    }
}
