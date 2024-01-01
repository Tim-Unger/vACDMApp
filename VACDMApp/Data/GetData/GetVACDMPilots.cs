using static VACDMApp.VACDMData.VACDMData;
using static VACDMApp.Windows.Views.LoadingView;

namespace VACDMApp.VACDMData
{
    public class VACDMPilotsData
    {
        public static async Task<List<VACDMPilot>> GetVACDMPilotsAsync()
        {
            Data.LoadingView.SetLabelText(LoadingStatus.VacdmData);

            var data = await Client.GetStringAsync(VacdmApiUrl);

            var dataList = JsonSerializer.Deserialize<List<VACDMPilot>>(data);

            if (dataList.Count == 0)
            {
                return Enumerable.Empty<VACDMPilot>().ToList();
            }

            //Remove VFR Flights
            return dataList
                .Where(x => x.FlightPlan.FlightRules == "I")
                .OrderBy(x => x.Vacdm.Eobt)
                .ToList();
        }
    }
}
