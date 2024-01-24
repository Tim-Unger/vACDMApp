using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static List<int> _days = new();

        private static List<Border> SplitAndRenderGrid(IEnumerable<VACDMPilot> pilots)
        {
            var sortByTime = pilots.OrderBy(x => x.Vacdm.Eobt).GroupBy(x => x.Vacdm.Eobt.Hour);

            var splitGrid = new List<Border>();

            foreach (var hourWindow in sortByTime)
            {
                var day = hourWindow.First().Vacdm.Eobt.Day;

                if (!_days.Contains(day))
                {
                    splitGrid.Add(RenderTimeSeperator(day, isDay: true));
                    _days.Add(day);
                }

                splitGrid.Add(RenderTimeSeperator(hourWindow.Key, isDay: false));

                splitGrid.AddRange(hourWindow.Select(RenderPilot));
            }

            //We clear the collection so that it is empty on refresh
            _days.Clear();
            return splitGrid;
        }
    }
}
