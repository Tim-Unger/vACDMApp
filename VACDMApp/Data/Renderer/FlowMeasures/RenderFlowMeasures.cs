using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        internal static List<Grid> Render()
        {
            //TODO Update Data
            var measures = VACDMData.Data.FlowMeasures;

            if (measures.Count == 0)
            {
                return new() { RenderNoMeasuresFound() };
            }

            return measures.Select(RenderMeasure).ToList();
        }
    }
}
