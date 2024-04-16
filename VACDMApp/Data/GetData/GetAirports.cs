using System.Text;

namespace VacdmApp.Data.GetData
{
    internal class AirportsData
    {
        internal static async Task<List<Airport>> GetAirportsAsync()
        {
            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("airports.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            return await JsonSerializer.DeserializeAsync<List<Airport>>(
                new MemoryStream(Encoding.UTF8.GetBytes(data))
            );
        }
    }
}
