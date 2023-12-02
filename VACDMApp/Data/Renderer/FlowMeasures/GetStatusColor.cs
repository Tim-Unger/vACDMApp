namespace VACDMApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        private static (Color Color, bool IsActive) GetStatusColor(FlowMeasure measure)
        {
            var now = DateTime.UtcNow;
            var startDate = measure.StartTime;
            var endDate = measure.EndTime;

            //Mesure is withdrawn
            if (measure.WithdrawnAt is not null)
            {
                return (Colors.Red, false);
            }

            //Now is later than EndDate => Measure is in the past
            if (now > endDate)
            {
                return (Colors.Red, false);
            }

            //Now is earlier than 24 hours before the Measure
            if (now < startDate.AddHours(-24))
            {
                return (Colors.Red, false);
            }

            //Now is less than 24 hours before the measure (no AddHours needed since we have the guard clause above
            if (now < startDate)
            {
                return (Colors.Yellow, false);
            }

            return (Colors.Green, true);
        }
    }
}
