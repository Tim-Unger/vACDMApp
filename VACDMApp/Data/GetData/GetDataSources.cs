using static VACDMApp.VACDMData.VACDMData;
using static VACDMApp.VACDMData.Data;
using System.Net.Http.Json;

namespace VACDMApp.Data
{
    public class DataSource
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class VaccDataSources
    {
        public static Task<List<DataSource>> GetDataSourcesAsync()
        {
            var options = new JsonSerializerOptions() { AllowTrailingCommas = true };

            var sources = Client.GetFromJsonAsync<List<DataSource>>(
                "https://raw.githubusercontent.com/Tim-Unger/vACDMDataSources/main/datasources.json",
                options
            );

            return sources;
        }
    }
}
