using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        internal static bool IsTestData = false;

        internal enum FilterKind
        {
            Airport,
            Airline,
            Cid,
            Callsign,
            Time
        }

        public static List<Border> Render(FilterKind? filterKind, string? filterValue)
        {
            var pilots = VACDMData.Data.VACDMPilots;

            var pilotsWithFP = VACDMData.Data.VACDMPilots
                //Only Pilots that have are in the Vatsim Datafeed
                .Where(x => VACDMData.Data.VatsimPilots.Exists(y => y.callsign == x.Callsign))
                //Only Pilots that have filed a flight plan
                .Where(
                    x =>
                        VACDMData.Data.VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan
                        != null
                );
            //Only pilots whose Eobt and TSAT lie within the future or max 5 minutes ago
            //.Where(
            //    x =>
            //        x.Vacdm.Eobt.Hour >= DateTime.UtcNow.AddHours(-1).Hour
            //&& x.Vacdm.Tsat >= DateTime.UtcNow.AddMinutes(-6)
            //);

            if (!pilotsWithFP.Any())
            {
                return new(1) { RenderNoFlightsFound() };
            }

            if (filterValue is not null)
            {
                //TODO Switch, if there are more than 2 options for the enum
                return filterKind switch
                {
                    FilterKind.Airport => SearchByAirport(pilotsWithFP, filterValue),
                    FilterKind.Airline => SearchByAirline(pilotsWithFP, filterValue),
                    FilterKind.Cid => SearchByCid(pilotsWithFP, filterValue),
                    FilterKind.Callsign => SearchByCallsign(pilotsWithFP, filterValue),
                    FilterKind.Time => SearchByTime(pilotsWithFP, filterValue)
                };
            }

            return SplitAndRenderGrid(pilotsWithFP);
        }

        private static List<Border> SearchByAirport(IEnumerable<VACDMPilot> pilotsWithFP, string filterValue)
        {
            var pilotsFromAirport = pilotsWithFP.Where(
                       x => x.FlightPlan.Departure == filterValue
                   );
            return SplitAndRenderGrid(pilotsFromAirport);
        }

        private static List<Border> SearchByAirline(IEnumerable<VACDMPilot> pilotsWithFP, string filterValue)
        {
            var pilotsFromAirline = pilotsWithFP.Where(
                        x => x.Callsign.StartsWith(filterValue.ToUpperInvariant())
                    );
            return SplitAndRenderGrid(pilotsFromAirline);
        }

        private static List<Border> SearchByCid(IEnumerable<VACDMPilot> pilotsWithFP, string filterValue)
        {
            //TryParse is done before the function is called
            var cid = int.Parse(filterValue);

            var vatsimPilotWithCid = VACDMData.Data.VatsimPilots.FirstOrDefault(x => x.cid == cid);

            if (vatsimPilotWithCid is null)
            {
                return new(1) { RenderNoFlightsFound() };
            }

            var pilotWithCid = pilotsWithFP.FirstOrDefault(x => x.Callsign == vatsimPilotWithCid.callsign);

            if (pilotWithCid is null)
            {
                return new(1) { RenderNoFlightsFound() };
            }

            var singleList = new List<VACDMPilot>(1) { pilotWithCid };
            return SplitAndRenderGrid(singleList);
        }

        private static List<Border> SearchByCallsign(IEnumerable<VACDMPilot> pilotsWithFP, string filterValue)
        {
            var pilot = pilotsWithFP.First(x => x.Callsign == filterValue.ToUpperInvariant());

            var singleList = new List<VACDMPilot>(1) { pilot };
            return SplitAndRenderGrid(singleList);
        }

        private static List<Border> SearchByTime(IEnumerable<VACDMPilot> pilotsWithFp, string filterValue)
        {
            var timeValue = int.Parse(filterValue);
            var pilots = pilotsWithFp.Where(x => x.Vacdm.Eobt.Hour == timeValue);

            return SplitAndRenderGrid(pilots);
        }
    }
}
