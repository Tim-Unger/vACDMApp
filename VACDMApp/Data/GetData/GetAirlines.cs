using System.Text;

namespace VacdmApp.Data
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
        }
    }
}
