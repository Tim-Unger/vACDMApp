using VacdmApp.Data;

namespace VacdmApp.Data
{
    public partial class SettingsData
    {
        //Make async
        public static async Task<Settings> ReadSettingsAsync() => await Task.Run(ReadSettings);

        private static Settings ReadSettings()
        {
            var cid = Preferences.Get("cid", 0);

            //TODO, reimplement? Or remove and keep current implementation
            var dataSources = Data.DataSources;

            var dataSource = Preferences.Get("data_source", null);

            var allowPush = Preferences.Get("allow_push", false);

            var pushMyFlightWindow = Preferences.Get("push_my_flight_window", false);

            var pushMyFlightTsatChanged = Preferences.Get("push_my_flight_tsat", false);

            var pushMyFlightStartup = Preferences.Get("push_my_flight_startup", false);

            var pushBookmarkWindow = Preferences.Get("push_bookmark_window", false);

            var pushBookmarkTsatChanged = Preferences.Get("push_bookmark_tsat", false);

            var pushBookmarkStartup = Preferences.Get("push_bookmark_startup", false);

            var pushMyFlightSlotUnconfirmed = Preferences.Get("push_my_flight_slot_unconfirmed", false);

            var pushFlowMeasures = Preferences.Get("push_flow_measures", false);

            var flowMesurePushFirs = Preferences.Get("flow_measure_push_firs", "");

            var updateAutomatically = Preferences.Get("update_automatically", true);

            var updateInterval = Preferences.Get("update_interval", 60);

            var updateVatsimDataAutomatically = Preferences.Get("update_vatsim", true);

            var updateVacdmDataAutomatically = Preferences.Get("update_vacdm", true);

            var updateEcfmpDataAutomatically = Preferences.Get("update_ecfmp", true);

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
                SendPushMyFlightSlotUnconfirmed = pushMyFlightSlotUnconfirmed,
                SendPushFlowMeasures = pushFlowMeasures,
                FlowMeasurePushFirs = flowMesurePushFirs,
                UpdateAutomatically = updateAutomatically,
                UpdateInterval = updateInterval,
                UpdateVatsimDataAutomatically = updateVatsimDataAutomatically,
                UpdateVacdmDataAutomatically = updateVacdmDataAutomatically,
                UpdateEcfmpDataAutomatically = updateEcfmpDataAutomatically
            };
        }
    }
}
