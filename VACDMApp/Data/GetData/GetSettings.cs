using VACDMApp.VACDMData;

namespace VACDMApp.Data
{
    public partial class SettingsData
    {
        public static async Task<Settings> ReadSettingsAsync()
        {
            var cid = Preferences.Get("cid", 0);

            var dataSources = VACDMData.Data.DataSources;

            var dataSource = Preferences.Get("data_source", "VATGER");

            var allowPush = Preferences.Get("allow_push", false);

            var pushMyFlightWindow = Preferences.Get("push_my_flight_window", false);

            var pushMyFlightTsatChanged = Preferences.Get("push_my_flight_tsat", false);

            var pushMyFlightStartup = Preferences.Get("push_my_flight_startup", false);

            var pushBookmarkWindow = Preferences.Get("push_bookmark_window", false);

            var pushBookmarkTsatChanged = Preferences.Get("push_bookmark_tsat", false);

            var pushBookmarkStartup = Preferences.Get("push_bookmark_startup", false);

            return new Settings()
            {
                Cid = cid != 0 ? cid : null,
                DataSource = dataSource,
                AllowPushNotifications = allowPush,
                SendPushMyFlightInsideWindow = pushMyFlightWindow,
                SendPushMyFlightTsatChanged = pushMyFlightTsatChanged,
                SendPushMyFlightStartup = pushMyFlightStartup,
                SendPushBookmarkFlightInsideWindow = pushBookmarkWindow,
                SendPushBookmarkTsatChanged = pushBookmarkTsatChanged,
                SendPushBookmarkStartup = pushBookmarkStartup,
            };
        }
    }
}
