namespace VacdmApp.Data
{
    public class GetVatsimData
    {
        public static async Task<VatsimData> GetVatsimDataAsync()
        {
            //TODO
#if DEBUG
            var data = await VacdmData.Client.GetStringAsync("https://api.tim-u.me/vatsim/data");
#else
            var data = await VacdmData.Client.GetStringAsync("https://data.vatsim.net/v3/vatsim-data.json");
#endif
            var dataJson = JsonSerializer.Deserialize<VatsimData>(data);

            return dataJson;
        }
    }
}
