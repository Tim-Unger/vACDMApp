using static VACDMApp.VACDMData.VACDMData;
using VACDMApp.DataFaker;

namespace VACDMApp.VACDMData
{
    internal class VACDMPilotsData
    {
        internal static async Task<List<VACDMPilot>> GetVACDMPilotsAsync()
        {
            var data = await Client.GetStringAsync(VacdmApiUrl);

            var dataList = JsonSerializer.Deserialize<List<VACDMPilot>>(data);

            if (dataList.Count == 0)
            {
#if DEBUG
                return VACDMFaker.FakePilots();
#endif
                return Enumerable.Empty<VACDMPilot>().ToList();
                ////TODO
                //dataList = JsonSerializer.Deserialize<List<VACDMPilot>>(TestData);

                //var random = new Random();

                //dataList.ForEach(x => x.Callsign = VACDMPilots[random.Next(0, VACDMPilots.Count)].Callsign);
            }

            //Remove VFR Flights
            return dataList.Where(x => x.FlightPlan.FlightRules == "I")
                    .OrderBy(x => x.Vacdm.Eobt)
                    .ToList();
        }
    }
}
