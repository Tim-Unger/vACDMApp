using System.Text;
using VACDMApp.VACDMData;

namespace VACDMApp.VACDMData
{
    internal class AirlinesData
    {
        internal static async Task<List<Airline>> GetAirlinesAsync()
        {
            var dataRaw = FileSystem.Current.OpenAppPackageFileAsync("airlines.json").Result;

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            return await JsonSerializer.DeserializeAsync<List<Airline>>(new MemoryStream(Encoding.UTF8.GetBytes(data)));
        }
    }
}
