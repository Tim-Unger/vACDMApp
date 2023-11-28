namespace VACDMApp.Data
{
    internal class FlowMeasure
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ident")]
        public string Ident { get; set; }

        [JsonPropertyName("event_id")]
        public string? EventId { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("starttime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endtime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("measure")]
        public Measure Measure { get; set; }

        [JsonPropertyName("filters")]
        public List<Filter> Filters { get; set; }

        [JsonPropertyName("notified_flight_information_regions")]
        public int[] notifiedFirs { get; set; }

        public List<Fir> NotifiedFirs { get; set; } = new();

        [JsonPropertyName("withdrawn_at")]
        public DateTime? WithdrawnAt { get; set; }
    }

    internal class Measure
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    internal class Filter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public List<string> Value { get; set; }
    }

    internal class Fir
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
