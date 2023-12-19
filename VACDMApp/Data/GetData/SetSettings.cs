namespace VACDMApp.Data
{
    public partial class SettingsData
    {
        //TODO
        internal static async Task SetSettingsAsync()
        {
            var settingsJson = JsonSerializer.Serialize(VACDMData.Data.Settings);

            var filePath = FileSystem.Current.AppDataDirectory;

            var writer = new StreamWriter($"{filePath}/settings.json");

            await writer.WriteAsync(settingsJson);

            writer.Dispose();

            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("settings.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();
        }
    }
}
