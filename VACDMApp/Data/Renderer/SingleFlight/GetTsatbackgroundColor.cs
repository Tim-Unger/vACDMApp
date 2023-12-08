namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        internal static Color GetTsatBackgroundColor(DateTime tsat, DateTime tobt)
        {
            //Startup Delay
            if (tsat > tobt.AddMinutes(5))
            {
                return Colors.Red;
            }

            return tsat.IsTsatInTheWindow() ? Colors.LimeGreen : Colors.Red;
        }
    }
}
