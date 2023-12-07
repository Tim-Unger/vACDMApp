using System.Net.Http.Json;
using System.Text;
using VACDMApp.VACDMData;

namespace VACDMApp.VACDMData
{
    internal class AirlinesData
    {
        internal static async Task<List<Airline>> GetAirlinesAsync()
        {
            var client = new HttpClient();

            return await client.GetFromJsonAsync<List<Airline>>("https://api.tim-u.me/airlines");
        }
    }
}
