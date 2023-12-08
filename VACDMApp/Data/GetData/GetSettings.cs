using VACDMApp.VACDMData;

namespace VACDMApp.Data
{
    internal class SettingsData
    {
        //TODO
        internal static async Task<Settings> ReadSettingsAsync()
        {
            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("settings.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            return JsonSerializer.Deserialize<Settings>(data);
        }
    }
}
