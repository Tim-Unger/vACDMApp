using System.Collections.Concurrent;
using System.Diagnostics;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static List<Border> SplitAndRenderGrid(IEnumerable<VACDMPilot> pilots)
        {
            var sortByTime = pilots.OrderBy(x => x.Vacdm.Eobt).GroupBy(x => x.Vacdm.Eobt.Hour);

            var splitGrid = new List<Border>();

            foreach (var hourWindow in sortByTime)
            {
                splitGrid.Add(RenderTimeSeperator(hourWindow.Key));

                //var hourPilots = new List<Border>();
                //var lockObj = new object();

                //Parallel.ForEach(hourWindow, x =>
                //{
                //    lock (lockObj)
                //    {
                //        hourPilots.Add(RenderPilot(x));
                //    }
                //});

                splitGrid.AddRange(hourWindow.Select(RenderPilot));
            }

            return splitGrid;
        }
    }
}
