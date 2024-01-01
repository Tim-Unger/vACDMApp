namespace VACDMApp.Data.OverridePermissions
{
    public class SendNotifications : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>()
            {
                (global::Android.Manifest.Permission.PostNotifications, true),
                (global::Android.Manifest.Permission.Internet, true),
                (global::Android.Manifest.Permission.AccessNetworkState, true)
            }.ToArray();
    }

    public class DefaultPermissions : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>()
            {
                (global::Android.Manifest.Permission.Internet, true),
                (global::Android.Manifest.Permission.AccessNetworkState, true),
            }.ToArray();
    }
}
