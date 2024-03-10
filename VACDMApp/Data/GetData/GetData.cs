using static VacdmApp.Data.Data;

namespace VacdmApp.Data
{
    internal class VacdmData
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
