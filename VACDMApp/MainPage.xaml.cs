using Plugin.LocalNotification;
using VACDMApp.Data;
using VACDMApp.Data.GetData;
using VACDMApp.Data.OverridePermissions;
using VACDMApp.Data.PushNotifications;
using VACDMApp.VACDMData;
using static VACDMApp.VACDMData.Data;

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

        private static bool _isLoadFinished = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await OnLoad(sender, e);
        }

        private async Task OnLoad(object sender, EventArgs e)
        {
            //This is just Internet and Network State, but we need to request it anyways,
            //since we are overriding the default Permissions Later on with the Push Notification Request
            await Permissions.RequestAsync<DefaultPermissions>();

            await GetAllData();

            await PushNotificationHandler.InitializeNotificationEvents(
                LocalNotificationCenter.Current
            );

            Mainview.Content = FlightsView;
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
            //TODO Accessibility Modifiers
            var dataSourcesTask = VaccDataSources.GetDataSourcesAsync();
            var settingsTask = SettingsData.ReadSettingsAsync();

            await Task.WhenAll(settingsTask, dataSourcesTask);

            DataSources = dataSourcesTask.Result;
            VACDMData.Data.Settings = settingsTask.Result;
            VACDMData.VACDMData.SetApiUrl();

            var dataTask = GetVatsimData.GetVatsimDataAsync();
            var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();
            var airlinesTask = AirlinesData.GetAirlinesAsync();
            var airportsTask = AirportsData.GetAsync();
            var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();
            var waypointsTask = GameWaypoints.GetWaypointsAsync();

            await Task.WhenAll(dataTask, vacdmTask, airlinesTask, measuresTask, airportsTask, waypointsTask);

            VatsimPilots = dataTask.Result.pilots.ToList();
            VACDMPilots = vacdmTask.Result;
            Airlines = airlinesTask.Result;
            Airports = airportsTask.Result;
            FlowMeasures = measuresTask.Result;
            Waypoints = waypointsTask.Result;
        }
    }
}
