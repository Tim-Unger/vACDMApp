namespace VACDMApp.VACDMData
{
    internal class GameWaypoints
    {
        internal static async Task<List<string>> GetWaypointsAsync()
        {
            var dataRaw = await FileSystem.Current.OpenAppPackageFileAsync("waypoints.json");

            var reader = new StreamReader(dataRaw);
            var data = reader.ReadToEnd();

            return JsonSerializer.Deserialize<List<string>>(data);
        }
    }
}
