using System.Net.Http.Json;
using System.Text;
using VACDMApp.VACDMData;

namespace VACDMApp.VACDMData
{
    public class AirlinesData
    {
        public static async Task<List<Airline>> GetAirlinesAsync()
        {
            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("airlines.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            return await JsonSerializer.DeserializeAsync<List<Airline>>(
                new MemoryStream(Encoding.UTF8.GetBytes(data))
            );

            //var client = new HttpClient();

            //return await client.GetFromJsonAsync<List<Airline>>("https://api.tim-u.me/airlines");
        }
    }
}
