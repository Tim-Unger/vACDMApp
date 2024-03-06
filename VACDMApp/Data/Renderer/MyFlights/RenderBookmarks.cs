using VacdmApp.Data;

namespace VacdmApp.Data.Renderer
{
    internal partial class Bookmarks
    {
        internal static List<Grid> Render(List<VacdmPilot> pilots) =>
            pilots.Select(RenderBookmark).ToList();
    }
}
