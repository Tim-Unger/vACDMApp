using static VACDMApp.VACDMData.Data;

namespace VACDMApp.DataFaker
{ 
    public partial class VACDMFaker
    {
        internal static void SetCallsigns()
        {
            var random = new Random();

            var vatsimPilots = VatsimPilots.Where(x => x.groundspeed < 20).ToList();

            var pilots = VACDMPilots;
            foreach (var pilot in pilots)
            {
                pilot.Callsign = vatsimPilots[random.Next(vatsimPilots.Count)].callsign;
            }

            VACDMPilots = pilots;
        }
    }
}
