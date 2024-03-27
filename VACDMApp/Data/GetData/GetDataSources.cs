using System.Net.Http.Json;
using System.Text;

namespace VacdmApp.Data
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

#if DEBUG
        private static readonly string _fallbackSources = """[{"name":"VATSIM Germany", "shortName":"VATGER", "url":"https://vacdm.vatsim-germany.org/api/v1/pilots/"},{ "name":"Test Data","shortName":"TEST","url":"https://vacdm.tim-u.me/api/v1/pilots"}]""";
#endif

        public static async Task<List<DataSource>> GetDataSourcesAsync()
        {
            var options = new JsonSerializerOptions() { AllowTrailingCommas = true };

            var sources = VacdmData.Client.GetFromJsonAsync<List<DataSource>>(
                "https://raw.githubusercontent.com/Tim-Unger/vACDMDataSources/main/datasources.json",
                options
            );

#if DEBUG
            var fallbackStream = new MemoryStream(Encoding.UTF8.GetBytes(_fallbackSources));

            var fallbackSources = JsonSerializer.DeserializeAsync<List<DataSource>>(fallbackStream);

            return await fallbackSources;
#else
            return await sources;
#endif
        }
    }
}
