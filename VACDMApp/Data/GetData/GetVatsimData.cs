using System.Net.Http.Json;
using static VACDMApp.VACDMData.VACDMData;

namespace VACDMApp.VACDMData
{
    public class GetVatsimData
    {
        public static async Task<VatsimData> GetVatsimDataAsync() =>
            await Client.GetFromJsonAsync<VatsimData>(
                "https://data.vatsim.net/v3/vatsim-data.json"
            );
    }
}
