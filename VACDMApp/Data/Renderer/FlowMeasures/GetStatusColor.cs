namespace VacdmApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        internal static (Color Color, MeasureStatus Status) GetStatus(FlowMeasure measure)
        {
            var now = DateTime.UtcNow;
            var startDate = measure.StartTime;
            var endDate = measure.EndTime;

            //Mesure is withdrawn
            if (measure.IsWithdrawn)
            {
                return (Colors.Red, MeasureStatus.Withdrawn);
            }

            //Now is later than EndDate => Measure is in the past
            if (now > endDate)
            {
                return (Colors.Red, MeasureStatus.Expired);
            }

            //Now is earlier than 24 hours before the Measure
            if (now < startDate.AddHours(-24))
            {
                return (Colors.Red, MeasureStatus.Notified);
            }

            //Now is less than 24 hours before the measure (no AddHours needed since we have the guard clause above)
            if (now < startDate)
            {
                return (Colors.Yellow, MeasureStatus.Notified);
            }

            return (Colors.Green, MeasureStatus.Active);
        }
    }
}
