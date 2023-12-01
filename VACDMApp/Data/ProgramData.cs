using VACDMApp.Data;
using VACDMApp.Windows;
using VACDMApp.Windows.Views;

namespace VACDMApp.VACDMData
{
    internal enum SenderPage
    {
        Default,
        SingleFlight,
        Vdgs,
        About
    }

    internal class Data
    {
        internal static List<VACDMPilot> VACDMPilots = new();

        internal static List<FlowMeasure> FlowMeasures = new();

        internal static List<Pilot> VatsimPilots = new();

        internal static List<Airline> Airlines = new();

        internal static MyFlightView MyFlightView = new();

        internal static FlightsView FlightsView = new();

        internal static FlowMeasuresView FlowMeasuresView = new();

        internal static SettingsView SettingsView = new();

        internal static Settings Settings = new();

        internal static AboutPage AboutPage = new();

        internal static SenderPage SenderPage;

        internal static object Sender;
    }
}
