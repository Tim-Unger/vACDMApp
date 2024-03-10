using static VacdmApp.Data.Data;
using static VacdmApp.Windows.Views.LoadingView;

namespace VacdmApp.Data
{
    public class VACDMPilotsData
    {
        public static async Task<List<VacdmPilot>> GetVACDMPilotsAsync()
        {
            Data.LoadingView.SetLabelText(LoadingStatus.VacdmData);

            var data = await VacdmData.Client.GetStringAsync(VacdmData.VacdmApiUrl);

            var dataList = JsonSerializer.Deserialize<List<VacdmPilot>>(data);

            if (dataList.Count == 0)
            {
                return Enumerable.Empty<VacdmPilot>().ToList();
            }

            //Remove VFR Flights
            return dataList
                .Where(x => x.FlightPlan.FlightRules == "I")
                .OrderBy(x => x.Vacdm.Eobt)
                .ToList();
        }
    }
}
