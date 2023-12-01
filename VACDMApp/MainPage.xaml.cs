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
        }

        //OnLoad
        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await GetAllData();
            Mainview.Content = FlightsView;

            //TODO
            //Data.Settings = VACDMData.VACDMData.ReadSettings();
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
            MyFlightButton.Source = currentPage == CurrentPage.MyFlight ? "plane.svg" : "plane_outline.svg";
            AllFlightsButton.Source = currentPage == CurrentPage.AllFlights ? "planes.svg" : "planes_outline.svg";
            FlowMeasuresButton.Source = currentPage == CurrentPage.FlowMeasures ? "flowmeasures.svg" : "flowmeasures_outline.svg";
            SettingsButton.Source = currentPage == CurrentPage.Settings ? "settings.svg" : "settings_outline.svg";
        }

        private static void SetSettings(Settings settings)
        {

        }

        internal static async Task GetAllData()
        {
            var dataTask = GetVatsimData.GetVatsimDataAsync();
            var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();
            var airlinesTask = AirlinesData.GetAirlinesAsync();
            var measuresTask = FlowMeasuresData.GetFlowMeasuresAsync();

            await Task.WhenAll(dataTask, vacdmTask, airlinesTask, measuresTask);

            VatsimPilots = dataTask.Result.pilots.ToList();
            VACDMPilots = vacdmTask.Result;
            Airlines = airlinesTask.Result;
            FlowMeasures = measuresTask.Result;
        }
    }
}