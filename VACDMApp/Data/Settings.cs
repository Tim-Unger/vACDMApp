namespace VACDMApp.VACDMData
{
    public class Settings
    {
        public int? Cid { get; set; }

        public string? DataSource { get; set; }

        public bool AllowPushNotifications { get; set; } = false;

        public bool SendPushMyFlightInsideWindow { get; set; } = false;

        public bool SendPushMyFlightTsatChanged { get; set; } = false;

        public bool SendPushMyFlightStartup { get; set; } = false;

        public bool SendPushBookmarkFlightInsideWindow { get; set; } = false;

        public bool SendPuishBookmarkTsatChanged { get; set; } = false;

        public bool SendPushBookmarkStartup { get; set; } = false;
    }
}
