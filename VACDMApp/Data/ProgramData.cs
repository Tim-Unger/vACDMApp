using VacdmApp.Data;
using VacdmApp.Windows.Views;

namespace VacdmApp.Data
{
    internal enum SenderPage
    {
        Default,
        SingleFlight,
        Vdgs,
        About,
        Airport,
        Time,
        FirSettings
    }

    internal class Data
    {
        internal static List<VacdmPilot> VacdmPilots = new();

        internal static List<FlowMeasure> FlowMeasures = new();

        internal static List<Fir> FlowMeasureFirs = new();

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

        internal static List<VacdmPilot> BookmarkedPilots = new();

        internal static SenderPage SenderPage;

        internal static object Sender;
    }
}
