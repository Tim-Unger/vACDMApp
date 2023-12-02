﻿namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        internal static List<Border> Render(string? airport)
        {
            var pilotsWithFP = VACDMApp.VACDMData.Data.VACDMPilots
                //Only Pilots that have are in the Vatsim Datafeed
                .Where(x => VACDMApp.VACDMData.Data.VatsimPilots.Exists(y => y.callsign == x.Callsign))
                //Only Pilots that have filed a flight plan
                .Where(
                    x => VACDMApp.VACDMData.Data.VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan != null
                )
                //Only pilots whose Eobt and TSAT lie within the future or max 5 minutes ago
                .Where(
                    x =>
                        x.Vacdm.Eobt.Hour >= DateTime.UtcNow.AddHours(-1).Hour
                        && x.Vacdm.Tsat >= DateTime.UtcNow.AddMinutes(-6)
                );

            if (pilotsWithFP.Count() == 0)
            {
                return new(1) { RenderNoFlightsFound() };
            }

            if (airport is not null)
            {
                var pilotsFromAirport = pilotsWithFP.Where(x => x.FlightPlan.Departure == airport);
                return SplitAndRenderGrid(pilotsFromAirport);
            }

            return SplitAndRenderGrid(pilotsWithFP);
        }
    }
}
