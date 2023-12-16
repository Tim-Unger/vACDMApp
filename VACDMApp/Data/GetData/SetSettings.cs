using System.Text;
namespace VACDMApp.Data
{
    public partial class SettingsData
    {
        internal static async Task SetSettingsAsync()
        {
            var target = Path.Combine(FileSystem.Current.AppDataDirectory, "settings.json");

            var settingsJson = JsonSerializer.Serialize(VACDMData.Data.Settings);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(settingsJson));

            var fileStream = new FileStream(target, FileMode.Open, FileAccess.Write);
            stream.CopyTo(fileStream);
        }
    }
}
