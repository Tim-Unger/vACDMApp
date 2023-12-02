using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Bookmarks
    {
        internal static List<Grid> Render(List<VACDMPilot> pilots) => pilots.Select(RenderBookmark).ToList();
    }
}
