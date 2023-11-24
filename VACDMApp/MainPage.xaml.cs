using VACDMApp.VACDMData;
using VACDMApp.Windows.BottomSheets;
using VACDMApp.Windows.Views;

namespace VACDMApp
{
    public partial class MainPage : ContentPage
    {
        public static List<VACDMPilot> VACDMPilots = new();

        public static List<Pilot> VatsimPilots = new();

        public static List<Airline> Airlines = new();

        public static FlightsView FlightsView = new();

        public static MyFlightView MyFlightView = new();

        public enum ViewPage
        {
            MyFlight,
            AllFlights,
            SingleFlight,
            Settings
        }

        public MainPage()
        {
            InitializeComponent();
        }

        //OnLoad
        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            VACDMPilots = VACDMData.VACDMData.Get();
            VatsimPilots = VACDMData.VACDMData.GetVatsimData().pilots.ToList();
            Airlines = VACDMData.VACDMData.GetAirlines();
            Mainview.Content = FlightsView;
        }

        private void MyFlightButton_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = MyFlightView;
        }

        private void AllFlights_Clicked(object sender, EventArgs e)
        {
            Mainview.Content = FlightsView;
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {

        }

        private void FlowMeasures_Clicked(object sender, EventArgs e)
        {

        }
    }
}