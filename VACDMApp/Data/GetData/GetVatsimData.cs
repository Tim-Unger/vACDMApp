using System.Net.Http.Json;
using static VACDMApp.VACDMData.VACDMData;
using static VACDMApp.Windows.Views.LoadingView;

namespace VACDMApp.VACDMData
{
    public class GetVatsimData
    {
        public static async Task<VatsimData> GetVatsimDataAsync()
        {
            Data.LoadingView.SetLabelText(LoadingStatus.VatsimData);

            var data = await Client.GetStringAsync("https://data.vatsim.net/v3/vatsim-data.json");

            return JsonSerializer.Deserialize<VatsimData>(data);
        }
    }
}
