using Plugin.LocalNotification;
using System.Net.NetworkInformation;
using VacdmApp.Data;
using VacdmApp.Data.GetData;
using VacdmApp.Data.OverridePermissions;
using VacdmApp.Data.PushNotifications;
using static VacdmApp.Data.Data;

namespace VacdmApp
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

        private bool _isFirstLoad = true;

        private MainPage _mainPage;

        private List<Task> _taskList = new();

        private CancellationTokenSource _progressCancellationTokenSource = new();

        private bool _isLoadSuccessfull = false;

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

            if (!_isLoadSuccessfull)
            {
                return;
            }

            await Task.Run(async () => 
            { 
                await DataHandler.RunAsync(); 
                await PushNotificationHandler.StartGlobalHandler(); 
            });

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

        internal async Task GetAllData()
        {
            var permissionsTask = Permissions.RequestAsync<DefaultPermissions>();
            permissionsTask.Wait();

            if (_isFirstLoad)
            {
                var dataSourcesTask = VaccDataSources.GetDataSourcesAsync();
                var settingsTask = SettingsData.ReadSettingsAsync();

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

                    _isLoadSuccessfull = false;
                    return;
                }

                if (DataSources.Count == 0)
                {
                    DataSources = dataSourcesTask.Result;
                }

                if (Data.Data.Settings is null)
                {
                    Data.Data.Settings = settingsTask.Result;

#if RELEASE
                    var testDataIndex = DataSources.FindIndex(x => x.ShortName == "TEST");

                    if (testDataIndex != -1)
                    {
                        DataSources.RemoveAt(testDataIndex);
                    }

                    var selectedSource = Preferences.Get("data_source", "VATGER");

                    if (selectedSource == "TEST")
                    {
                        Data.Data.Settings.DataSource = null;
                    }

                    if (Data.Data.Settings.DataSource is not null)
                    {
                        VacdmData.SetApiUrl();
                    }
#endif
                }
            }

            if (_taskList.Count != 0)
            {
                _taskList.Clear();
            }

            var dataTask = GetVatsimData.GetVatsimDataAsync();
            _taskList.Add(dataTask);

            VacdmData.SetApiUrl();
            var vacdmTask = VacdmPilotsData.GetVacdmPilotsAsync();
            _taskList.Add(vacdmTask);

            var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();
            _taskList.Add(measuresTask);

            var airlinesTask = AirlinesData.GetAirlinesAsync();
            var airportsTask = AirportsData.GetAirportsAsync();

            if (_isFirstLoad)
            {
                _taskList.Add(airlinesTask);
                _taskList.Add(airportsTask);
            }

            var normalTasks = Task.WhenAll(_taskList);
            var progressTask = GetProgressCountAsync();

            try
            {
                await progressTask;
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

                _isLoadSuccessfull = false;

                return;
            }

            _taskList.Clear();

            VatsimPilots = dataTask.Result.pilots.ToList();
            VacdmPilots = vacdmTask.Result;
            FlowMeasures = measuresTask.Result.FlowMeasures;
            FlowMeasureFirs = measuresTask.Result.Firs;

            if (_isFirstLoad)
            {
                Airlines = airlinesTask.Result;
                Airports = airportsTask.Result;
            }

            _isLoadSuccessfull = true;

            _isFirstLoad = false;
        }

        private static bool HasUserInternet()
        {
            var ping = new Ping();

            var cloudflarePing = ping.Send("1.1.1.1");

            if (cloudflarePing.Status != IPStatus.Success)
            {
                //We try Google just in case Cloudflare is down (haha good joke)
                var googlePing = ping.Send("8.8.8.8");

                if (googlePing.Status != IPStatus.Success)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        private async Task GetProgressCountAsync()
        {
            var taskCount = _taskList.Count;

            while (!_progressCancellationTokenSource.IsCancellationRequested)
            {
                var completedTasks = _taskList.Count(x => x.IsCompletedSuccessfully);

                var progressDouble = (double)completedTasks / taskCount;

                var progress = Math.Round(progressDouble * 100, 0);

                if (progress == 100)
                {
                    LoadingLabel.Text = $"{progress}%";
                    await Task.Delay(50);
                    _progressCancellationTokenSource.Cancel();
                    return;
                }

                LoadingLabel.Text = $"{progress}%";

                //To make this method actually async (slightly yikes)
                await Task.Delay(5);
            }
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

        private void ContentPage_Unloaded(object sender, EventArgs e)
        {
            //DataHandler.Cancel();
        }
    }
}
