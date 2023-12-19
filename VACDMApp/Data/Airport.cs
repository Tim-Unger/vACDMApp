namespace VACDMApp.Data
{
    internal class Airport
    {
        public string Icao { get; set; }
        public string Iata { get; set; }
        public string Name { get; set; }
        public List<decimal> Coordinates { get; set; } = new List<decimal>(2);
        public long Elevation { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool HasScheduledService { get; set; }
    }
}
