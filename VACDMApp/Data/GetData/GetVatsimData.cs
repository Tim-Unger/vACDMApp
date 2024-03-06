using System.Net.Http.Json;
using static VacdmApp.Data.Data;
using static VacdmApp.Windows.Views.LoadingView;

namespace VacdmApp.Data
{
    public class GetVatsimData
    {
        public static async Task<VatsimData> GetVatsimDataAsync()
        {
            Data.LoadingView.SetLabelText(LoadingStatus.VatsimData);

            var data = await VacdmData.Client.GetStringAsync("https://data.vatsim.net/v3/vatsim-data.json");

            return JsonSerializer.Deserialize<VatsimData>(data);
        }
    }
}
