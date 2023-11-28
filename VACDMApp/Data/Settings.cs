using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VACDMApp.VACDMData
{
    internal class Settings
    {
        [JsonPropertyName("cid")]
        internal int? Cid { get; set; }
    }
}
