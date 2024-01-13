using VACDMApp.Data;
using VACDMApp.Windows.Views;

namespace VACDMApp.VACDMData
{
    internal enum SenderPage
    {
        Default,
        SingleFlight,
        Vdgs,
        About,
        Airport,
        Time,
    }

    internal class Data
    {
        internal static List<VACDMPilot> VACDMPilots = new();

        internal static List<FlowMeasure> FlowMeasures = new();

        internal static List<Pilot> VatsimPilots = new();

        internal static List<Airline> Airlines = new();

        internal static List<DataSource> DataSources = new();

        internal static List<Airport> Airports = new();

        internal static MyFlightView MyFlightView = new();

        internal static FlightsView FlightsView = new();

        internal static FlowMeasuresView FlowMeasuresView = new();

        internal static SettingsView SettingsView = new();

        internal static Settings? Settings = null;

        internal static AboutPage AboutPage = new();

        internal static List<VACDMPilot> BookmarkedPilots = new();

        internal static List<string> Waypoints = new();

        internal static SenderPage SenderPage;

        internal static LoadingView LoadingView = new();

        internal static object Sender;
    }
}
