namespace VacdmApp.Data
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

        public bool SendPushBookmarkTsatChanged { get; set; } = false;

        public bool SendPushBookmarkStartup { get; set; } = false;

        public bool SendPushMyFlightSlotUnconfirmed { get; set; } = false;

        public bool SendPushFlowMeasures { get; set; } = false;

        public string FlowMeasurePushFirs { get; set; } = "";

        public bool UpdateAutomatically { get; set; } = true;
    }
}
