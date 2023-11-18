using VACDMApp.VACDMData;
using VACDMApp.VACDMData.Renderer;
using VACDMApp.Windows.BottomSheets;
using VACDMApp.Windows.Views;

namespace VACDMApp
{
    public partial class MainPage : ContentPage
    {
        public static List<VACDMPilot> VACDMPilots = new();

        public static List<Pilot> VatsimPilots = new();

        public static List<Airline> Airlines = new();
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
            GetNearestTime();
            Mainview.Content = new FlightsView();
        }

        private void AirportsButton_Clicked(object sender, EventArgs e)
        {
            var airportsSheet = new AirportsBottomSheet();

            airportsSheet.ShowAsync();
        }

        private void DayButton_Clicked(object sender, EventArgs e)
        {
            //FlightsDatePicker.IsVisible = true;
            //FlightsDatePicker.
        }

        //TODO
        private void TimeButton_Clicked(object sender, EventArgs e)
        {
            
        }

        private void TimeFormatButton_Clicked(object sender, EventArgs e)
        {

        }

        private void GetNearestTime()
        {
            var now = DateTime.UtcNow;

            FlightsTimePicker.Time = new TimeSpan(now.Hour, 0, 0);
        }
    }
}