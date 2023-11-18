namespace VACDMApp.VACDMData
{
    public class VACDMPilot
    {
        [JsonPropertyName("position")]
        public Position Position { get; set; }

        [JsonPropertyName("vacdm")]
        public Vacdm Vacdm { get; set; }

        [JsonPropertyName("flightplan")]
        public FlightPlan FlightPlan { get; set; }

        [JsonPropertyName("clearance")]
        public Clearance Clearance { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("callsign")]
        public string Callsign { get; set; }

        [JsonPropertyName("hasBooking")]
        public bool HasBooking { get; set; }

        [JsonPropertyName("inactive")]
        public bool IsInactive { get; set; }

        [JsonPropertyName("measures")]
        public object[] Measures { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("__v")]
        public int V { get; set; }
    }
}
