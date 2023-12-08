namespace VACDMApp.VACDMData
{
    internal class Settings
    {
        [JsonPropertyName("cid")]
        public int? Cid { get; set; }

        [JsonPropertyName("dataSource")]
        public string? DataSource { get; set; }
    }
}
