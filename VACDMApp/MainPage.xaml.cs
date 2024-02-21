using Plugin.LocalNotification;
using System.Net.NetworkInformation;
using VACDMApp.Data;
using VACDMApp.Data.GetData;
using VACDMApp.Data.OverridePermissions;
using VACDMApp.Data.PushNotifications;
using VACDMApp.VACDMData;
using static VACDMApp.VACDMData.Data;
using static VACDMApp.Windows.Views.LoadingView;

namespace VACDMApp
{
    public partial class MainPage : ContentPage
    {
        public enum CurrentPage
        {
            MyFlight,
            AllFlights,
            FlowMeasures,
            Settings
        }

        private static bool _isFirstLoad = true;

        private static MainPage _mainPage;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            _mainPage = this;
            await OnLoad();
        }

        private async Task OnLoad()
        {
            //TODO Fix Content getting fucked up when reloading
            ErrorGrid.IsVisible = false;
            NoInternetGrid.IsVisible = false;

            SetButtons(false);
            Mainview.Content = null;

            //This is just Internet and Network State, but we need to request it anyways,
            //since we are overriding the default Permissions Later on with the Push Notification Request
            var permissionsTask = Permissions.RequestAsync<DefaultPermissions>();
            permissionsTask.Wait();

            var hasUserInternet = HasUserInternet();

            if (!hasUserInternet)
            {
                NoInternetGrid.IsVisible = true;
                Mainview.IsVisible = false;
                return;
            }

            NoInternetGrid.IsVisible = false;
            Mainview.IsVisible = true;
            LoadingGrid.IsVisible = true;

            await GetAllData();

            LoadingGrid.IsVisible = false;
            Mainview.Content = FlightsView;
            SetButtons(true);

            await PushNotificationHandler.StartGlobalHandler();

            await PushNotificationHandler.InitializeNotificationEvents(
               LocalNotificationCenter.Current
           );
        }

        private void MyFlightButton_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = MyFlightView;
            SetButton(CurrentPage.MyFlight);
        }

        private void AllFlightsButton_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = FlightsView;
            SetButton(CurrentPage.AllFlights);
        }

        private void FlowMeasuresButton_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = FlowMeasuresView;
            SetButton(CurrentPage.FlowMeasures);
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = SettingsView;
            SetButton(CurrentPage.Settings);
        }

        private void SetButton(CurrentPage currentPage)
        {
            MyFlightImage.Source =
                currentPage == CurrentPage.MyFlight ? "plane.svg" : "plane_outline.svg";
            AllFlightsImage.Source =
                currentPage == CurrentPage.AllFlights ? "planes.svg" : "planes_outline.svg";
            FlowmeasuresImage.Source =
                currentPage == CurrentPage.FlowMeasures
                    ? "flowmeasures.svg"
                    : "flowmeasures_outline.svg";
            SettingsImage.Source =
                currentPage == CurrentPage.Settings ? "settings.svg" : "settings_outline.svg";
        }

        internal static async Task GetAllData()
        {
            var permissionsTask = Permissions.RequestAsync<DefaultPermissions>();
            permissionsTask.Wait();

            if (_isFirstLoad)
            {
                var dataSourcesTask = VaccDataSources.GetDataSourcesAsync();
                var settingsTask = SettingsData.ReadSettingsAsync();

                LoadingView.SetLabelText(LoadingStatus.Settings);

                var firstLoadTask = Task.WhenAll(settingsTask, dataSourcesTask);
                try
                {
                    await firstLoadTask;
                }
                catch (Exception ex)
                {
#if DEBUG
                    _mainPage.DebugErrorLabel.IsVisible = true;
                    _mainPage.DebugErrorLabel.Text = ex.Message;
#endif
                    _mainPage.ErrorGrid.IsVisible = true;
                    _mainPage.Mainview.IsVisible = false;
                    return;
                }

                if (DataSources.Count == 0)
                {
                    DataSources = dataSourcesTask.Result;

                    //TODO add back
//#if RELEASE
//                    var testDataIndex = DataSources.FindIndex(x => x.ShortName == "TEST");
//                    DataSources.RemoveAt(testDataIndex);
//#endif
                }

                if (VACDMData.Data.Settings is null)
                {
                    VACDMData.Data.Settings = settingsTask.Result;
                    VACDMData.VACDMData.SetApiUrl();
                }
            }

            var taskList = new List<Task>();

            var dataTask = GetVatsimData.GetVatsimDataAsync();
            taskList.Add(dataTask);

            var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();
            taskList.Add(vacdmTask);

            var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();
            taskList.Add(measuresTask);

            var airlinesTask = AirlinesData.GetAirlinesAsync();
            var airportsTask = AirportsData.GetAirportsAsync();
            var waypointsTask = GameWaypoints.GetWaypointsAsync();

            if (_isFirstLoad)
            {
                taskList.Add(airlinesTask);
                taskList.Add(airportsTask);
                taskList.Add(waypointsTask);
            }

            var normalTasks = Task.WhenAll(taskList);

            try
            {
                await normalTasks;
            }
            catch (Exception ex)
            {
#if DEBUG
                _mainPage.DebugErrorLabel.IsVisible = true;
                _mainPage.DebugErrorLabel.Text = ex.Message;
#endif
                _mainPage.ErrorGrid.IsVisible = true;
                _mainPage.Mainview.IsVisible = false;
                return;
            }

            VatsimPilots = dataTask.Result.pilots.ToList();
            VACDMPilots = vacdmTask.Result;
            FlowMeasures = measuresTask.Result.FlowMeasures;
            FlowMeasureFirs = measuresTask.Result.Firs;

            if (_isFirstLoad)
            {
                Airlines = airlinesTask.Result;
                Airports = airportsTask.Result;
                Waypoints = waypointsTask.Result;
            }

            _isFirstLoad = false;
        }

        private bool HasUserInternet()
        {
            var ping = new Ping();

            var cloudflarePing = ping.Send("1.1.1.1");

            if(cloudflarePing.Status != IPStatus.Success)
            {
                //We try Google just in case Cloudflare is down
                var googlePing = ping.Send("8.8.8.8");

                if(googlePing.Status != IPStatus.Success)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        private async void TryAgainButton_Pressed(object sender, EventArgs e)
        {
            await OnLoad();
        }

        private void SetButtons(bool isEnabled)
        {
            MyFlightButton.IsEnabled = isEnabled;
            AllFlightsButton.IsEnabled = isEnabled;
            FlowMeasuresButton.IsEnabled = isEnabled;
            SettingsButton.IsEnabled = isEnabled;
        }
    }
}
