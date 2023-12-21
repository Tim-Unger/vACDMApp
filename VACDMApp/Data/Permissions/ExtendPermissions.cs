namespace VACDMApp.Data.OverridePermissions
{
    public class SendNotifications : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>()
            {
                (global::Android.Manifest.Permission.PostNotifications, true)
            }.ToArray();
    }
}
