using System.Net.Http.Json;
using VACDMApp.Data;
using static VACDMApp.VACDMData.VACDMData;

namespace VACDMApp.VACDMData
{
    internal class FlowMeasuresData
    {
        internal static Task<List<FlowMeasure>> GetFlowMeasuresAsync()
        {
            //TODO
           //var firs = GetFirs();

            var measures = Client.GetFromJsonAsync<List<FlowMeasure>>("https://ecfmp.vatsim.net/api/v1/flow-measure");

            //measures.ForEach(x => x.NotifiedFirs = x.notifiedFirs.Select(y => firs.First(z => z.Id == y)).ToList());

            return measures;
        }

        //TODO
        private static async Task<List<Fir>> GetFirs() => await Client.GetFromJsonAsync<List<Fir>>("https://ecfmp.vatsim.net/api/v1/flight-information-region");
    }
}
