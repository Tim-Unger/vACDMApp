using System.Text;
using static VacdmApp.Windows.Views.LoadingView;


namespace VacdmApp.Data.GetData
{
    internal class AirportsData
    {
        internal static async Task<List<Airport>> GetAirportsAsync()
        {
            Data.LoadingView.SetLabelText(LoadingStatus.Airports);

            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("airports.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return await JsonSerializer.DeserializeAsync<List<Airport>>(
                new MemoryStream(Encoding.UTF8.GetBytes(data))
            );
        }
    }
}
