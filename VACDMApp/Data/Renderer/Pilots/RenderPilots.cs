using VacdmApp.Data;

namespace VacdmApp.Data.Renderer
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
            var pilots = Data.VacdmPilots;

            var pilotsWithFP = Data.VacdmPilots
                //Only Pilots that have are in the Vatsim Datafeed
                .Where(x => Data.VatsimPilots.Exists(y => y.callsign == x.Callsign))
                //Only Pilots that have filed a flight plan
                .Where(
                    x =>
                        Data.VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan
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

        private static List<Border> SearchByAirport(
            IEnumerable<VacdmPilot> pilotsWithFP,
            string filterValue
        )
        {
            var pilotsFromAirport = pilotsWithFP.Where(x => x.FlightPlan.Departure == filterValue);
            return SplitAndRenderGrid(pilotsFromAirport);
        }

        private static List<Border> SearchByAirline(
            IEnumerable<VacdmPilot> pilotsWithFP,
            string filterValue
        )
        {
            var pilotsFromAirline = pilotsWithFP.Where(
                x => x.Callsign.StartsWith(filterValue.ToUpperInvariant())
            );
            return SplitAndRenderGrid(pilotsFromAirline);
        }

        private static List<Border> SearchByCid(
            IEnumerable<VacdmPilot> pilotsWithFP,
            string filterValue
        )
        {
            //TryParse is done before the function is called
            var cid = int.Parse(filterValue);

            var vatsimPilotsWithCid = Data.VatsimPilots.Where(x => x.cid.ToString().StartsWith(cid.ToString()));

            if (vatsimPilotsWithCid is null)
            {
                return new(1) { RenderNoFlightsFound() };
            }

            var pilotsWithCid = new List<VacdmPilot>();

            foreach (var vatsimPilot in vatsimPilotsWithCid)
            {
                var vacdmPilot = Data.VacdmPilots.FirstOrDefault(x => x.Callsign == vatsimPilot.callsign);

                if (vacdmPilot is null)
                {
                    continue;
                }

                pilotsWithCid.Add(vacdmPilot);
            }

            if (pilotsWithCid.Count == 0)
            {
                return new(1) { RenderNoFlightsFound() };
            }

            return SplitAndRenderGrid(pilotsWithCid);
        }

        private static List<Border> SearchByCallsign(
            IEnumerable<VacdmPilot> pilotsWithFP,
            string filterValue
        )
        {
            var pilot = pilotsWithFP.First(x => x.Callsign == filterValue.ToUpperInvariant());

            var singleList = new List<VacdmPilot>(1) { pilot };
            return SplitAndRenderGrid(singleList);
        }

        private static List<Border> SearchByTime(
            IEnumerable<VacdmPilot> pilotsWithFp,
            string filterValue
        )
        {
            var timeValue = int.Parse(filterValue);
            var pilots = pilotsWithFp.Where(x => x.Vacdm.Eobt.Hour == timeValue);

            return SplitAndRenderGrid(pilots);
        }
    }
}
