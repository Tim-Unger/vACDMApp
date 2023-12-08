using Plugin.LocalNotification;
using VACDMApp.Data;
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

        public MainPage()
        {
            InitializeComponent();
            PushNotificationHandler.InitializeNotificationEvents(LocalNotificationCenter.Current);
        }

        //OnLoad
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await GetAllData();
            Mainview.Content = FlightsView;

            //TODO
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

        private static void SetSettings(Settings settings)
        {
            throw new NotImplementedException();
        }

        internal static async Task GetAllData()
        {
            var dataTask = GetVatsimData.GetVatsimDataAsync();
            var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();
            var airlinesTask = AirlinesData.GetAirlinesAsync();
            var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();
            var dataSourcesTask = VaccDataSources.GetDataSourcesAsync();
            var settingsTask = SettingsData.ReadSettingsAsync();

            await Task.WhenAll(dataTask, vacdmTask, airlinesTask, measuresTask, dataSourcesTask, settingsTask);

            VatsimPilots = dataTask.Result.pilots.ToList();
            VACDMPilots = vacdmTask.Result;
            Airlines = airlinesTask.Result;
            FlowMeasures = measuresTask.Result;
            DataSources = dataSourcesTask.Result;
            VACDMData.Data.Settings = settingsTask.Result;
        }
    }
}