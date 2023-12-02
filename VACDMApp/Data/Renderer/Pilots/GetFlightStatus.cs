using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        public static string GetFlightStatus(VACDMPilot pilot)
        {
            var vatsimPilot = VACDMData.Data.VatsimPilots.First(x => x.callsign == pilot.Callsign);
            if (vatsimPilot.groundspeed > 50)
            {
                return "Departed";
            }

            var vacdm = pilot.Vacdm;

            if (vacdm.Asrt.Year == 1969)
            {
                return "Planned";
            }

            if (vacdm.Pbg.Year == 1969)
            {
                return "Startup given";
            }

            if (vacdm.Txg.Year == 1969)
            {
                return "Offblock";
            }

            return "Taxi Out";
        }
    }
}
