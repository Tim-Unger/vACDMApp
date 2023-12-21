using Android.Telephony;
using Plugin.LocalNotification;
using VACDMApp.Data;
using VACDMApp.Data.GetData;
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

        //OnLoad
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await GetAllData();

            //await PushNotificationHandler.InitializeNotificationEvents(LocalNotificationCenter.Current);

            Mainview.Content = FlightsView;
        }

        private void MyFlightButton_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = MyFlightView;
            MyFlightView.RenderBookmarks();
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
            MyFlightImage.Source = currentPage == CurrentPage.MyFlight ? "plane.svg" : "plane_outline.svg";
            AllFlightsImage.Source = currentPage == CurrentPage.AllFlights ? "planes.svg" : "planes_outline.svg";
            FlowmeasuresImage.Source = currentPage == CurrentPage.FlowMeasures ? "flowmeasures.svg" : "flowmeasures_outline.svg";
            SettingsImage.Source = currentPage == CurrentPage.Settings ? "settings.svg" : "settings_outline.svg";
        }

        internal static async Task GetAllData()
        {
            //TODO Accessibility Modifiers
            var dataSourcesTask = VaccDataSources.GetDataSourcesAsync();
            var settingsTask = SettingsData.ReadSettingsAsync();

            await Task.WhenAll(dataSourcesTask, settingsTask);

            DataSources = dataSourcesTask.Result;
            VACDMData.Data.Settings = settingsTask.Result;
            VACDMData.VACDMData.SetApiUrl();


            var dataTask = GetVatsimData.GetVatsimDataAsync();
            var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();
            var airlinesTask = AirlinesData.GetAirlinesAsync();
            var airportsTask = AirportsData.GetAsync();
            var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();

            await Task.WhenAll(dataTask, vacdmTask, airlinesTask, measuresTask, airportsTask);

            VatsimPilots = dataTask.Result.pilots.ToList();
            VACDMPilots = vacdmTask.Result;
            Airlines = airlinesTask.Result;
            Airports = airportsTask.Result;
            FlowMeasures = measuresTask.Result;
        }
    }
}