namespace VacdmApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        internal static List<Grid> Render(bool showActive, bool showNotified, bool showExpired, bool showWithdrawn)
        {
            //TODO Implement
            var measures = Data.FlowMeasures;

            if (measures.Count == 0)
            {
                return new() { RenderNoMeasuresFound() };
            }

            var allowedStatuses = new List<MeasureStatus>();

            if (showActive)
            {
                allowedStatuses.Add(MeasureStatus.Active);
            }

            if(showNotified)
            {
                allowedStatuses.Add(MeasureStatus.Notified);
            }

            if (showExpired)
            {
                allowedStatuses.Add(MeasureStatus.Expired);
            }

            if (showWithdrawn)
            {
                allowedStatuses.Add(MeasureStatus.Withdrawn);
            }

            var filteredMeasures = FilterMeasures(allowedStatuses);

            return filteredMeasures.Select(RenderMeasure).ToList();
        }

        private static List<FlowMeasure> FilterMeasures(List<MeasureStatus> allowedStatuses) => Data.FlowMeasures.Where(x => allowedStatuses.Any(y => x.MeasureStatus == y)).ToList();
    }
}
