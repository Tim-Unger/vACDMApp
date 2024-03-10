using VacdmApp.Data;

namespace VacdmApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static List<Border> SplitAndRenderGrid(IEnumerable<VacdmPilot> pilots)
        {
            var sortByTime = pilots.OrderBy(x => x.Vacdm.Eobt).GroupBy(x => x.Vacdm.Eobt.Hour);

            var splitGrid = new List<Border>();

            foreach (var hourWindow in sortByTime)
            {
                splitGrid.Add(RenderTimeSeperator(hourWindow.First().Vacdm.Eobt));

                splitGrid.AddRange(hourWindow.Select(RenderPilot));
            }

            return splitGrid;
        }
    }
}
