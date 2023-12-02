namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Color GetTsatBackgroundColor(DateTime tsat, DateTime tobt)
        {
            var now = DateTime.UtcNow;

            //Startup Delay
            if (tsat > tobt.AddMinutes(5))
            {
                return Colors.Red;
            }

            //IN the Window (+/-5)
            if (tsat.AddMinutes(-5) <= now && tsat.AddMinutes(5) >= now)
            {
                return Colors.LimeGreen;
            }

            //Outside of the Window
            return Colors.Red;
        }
    }
}
