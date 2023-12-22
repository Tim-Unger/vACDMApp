using VACDMApp.VACDMData;

namespace VACDMApp.Data
{
    public partial class SettingsData
    {
        //TODO
        public static async Task<Settings> ReadSettingsAsync()
        {
            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("settings.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Deserialize<Settings>(data, options);
        }
    }
}
