using System.Net.Http.Json;
using System.Text;
using VACDMApp.Data;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.VACDMData
{
    internal class VACDMData
    {
        internal static readonly HttpClient Client = new();

        internal static readonly string VacdmApiUrl = "https://vacdm.vatsim-germany.org/api/v1/pilots/";
    }
}
