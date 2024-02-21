namespace VACDMApp.Data
{
    public enum MeasureType
    {
        MDI,
        ADI,
        FlightsPerHour,
        MIT,
        MaxIas,
        MaxMach,
        IasReduction,
        MachReduction,
        Prohibit,
        GroundStop,
        MandatoryRoute
    }

    public enum MeasureStatus
    {
        Active,
        Notified,
        Expired,
        Withdrawn
    }

    public class FlowMeasure
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ident")]
        public string Ident { get; set; }

        [JsonPropertyName("event_id")]
        public int? EventId { get; set; }

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

        public MeasureStatus MeasureStatus { get; set; } 

        [JsonPropertyName("withdrawn_at")]
        public DateTime? WithdrawnAt { get; set; }

        public bool IsWithdrawn { get; set; } = false;
    }

    public class Measure
    {
        [JsonPropertyName("type")]
        public string TypeRaw { get; set; }

        public MeasureType MeasureType { get; set; }

        public string MeasureTypeString { get; set; }

        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }

    public class Filter
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }

    public class Fir
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
