using static VACDMApp.VACDMData.Data;

namespace VACDMApp.VACDMData
{
    internal class VACDMData
    {
        internal static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(10) };

        internal static string VacdmApiUrl = "";

        internal static void SetApiUrl() 
        {
            var selectedDataSourceName = Data.Settings.DataSource;

            var dataSourceUrl = DataSources.First(x => x.ShortName == selectedDataSourceName).Url;

            VacdmApiUrl = dataSourceUrl;
        }
    }
}
