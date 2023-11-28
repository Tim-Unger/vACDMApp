namespace VACDMApp.VACDMData
{
    public class Clearance
    {
        [JsonPropertyName("dep_rwy")]
        public string DepRwy { get; set; }

        [JsonPropertyName("sid")]
        public string Sid { get; set; }

        [JsonPropertyName("initial_climb")]
        public string InitialClimb { get; set; }

        [JsonPropertyName("assigned_squawk")]
        public string AssignedSquawk { get; set; }

        [JsonPropertyName("current_squawk")]
        public string CurrentSquawk { get; set; }
    }
}
