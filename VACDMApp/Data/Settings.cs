namespace VACDMApp.VACDMData
{
    public class Settings
    {
        [JsonPropertyName("cid")]
        public int? Cid { get; set; }

        [JsonPropertyName("dataSource")]
        public string? DataSource { get; set; }
    }
}
