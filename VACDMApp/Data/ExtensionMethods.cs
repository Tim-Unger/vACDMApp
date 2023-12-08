namespace VACDMApp.Data
{
    public static class ExtensionMethods
    {
        public static bool IsTsatInTheWindow(this DateTime tsat)
        {
            var now = DateTime.UtcNow;

            if (tsat.AddMinutes(-5) <= now && tsat.AddMinutes(5) >= now)
            {
                return true;
            }

            return false;
        }
    }
}
