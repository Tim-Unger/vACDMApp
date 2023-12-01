using VACDMApp.VACDMData;

namespace VACDMApp.Data.GetData
{
    internal class GetSettings
    {
        //TODO
        internal static Settings ReadSettings()
        {
            var dataRaw = FileSystem.Current.OpenAppPackageFileAsync("settings.json").Result;

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            Settings settings = JsonSerializer.Deserialize<Settings>(data);

            return settings;
        }
    }
}
