namespace VACDMApp.VACDMData
{
    public class Vacdm
    {
        [JsonPropertyName("eobt")]
        public DateTime Eobt { get; set; }

        [JsonPropertyName("tobt")]
        public DateTime Tobt { get; set; }

        [JsonPropertyName("tobt_state")]
        public string TobtState { get; set; }

        [JsonPropertyName("exot")]
        public int Exot { get; set; }

        [JsonPropertyName("manual_exot")]
        public bool IsManualExot { get; set; }

        [JsonPropertyName("tsat")]
        public DateTime Tsat { get; set; }

        [JsonPropertyName("ctot")]
        public DateTime Ctot { get; set; }

        [JsonPropertyName("ttot")]
        public DateTime Ttot { get; set; }

        [JsonPropertyName("asrt")]
        public DateTime Asrt { get; set; }

        [JsonPropertyName("aort")]
        public DateTime Aort { get; set; }

        [JsonPropertyName("Asat")]
        public DateTime Asat { get; set; }

        [JsonPropertyName("Aobt")]
        public DateTime Aobt { get; set; }

        [JsonPropertyName("delay")]
        public int DelayMin { get; set; }

        [JsonPropertyName("prio")]
        public int Priority { get; set; }

        [JsonPropertyName("sug")]
        public DateTime Sug { get; set; }

        [JsonPropertyName("pbg")]
        public DateTime Pbg { get; set; }

        [JsonPropertyName("txg")]
        public DateTime Txg { get; set; }

        [JsonPropertyName("taxizone")]
        public string TaxiZone { get; set; }

        [JsonPropertyName("taxizoneIsTaxiout")]
        public bool IsTaxizoneTaxiout { get; set; }

        [JsonPropertyName("blockAssignment")]
        public DateTime BlockAssignment { get; set; }

        [JsonPropertyName("blockId")]
        public int BlockId { get; set; }

        [JsonPropertyName("block_rwy_designator")]
        public string BlockRwyDesignator { get; set; }
    }
}
