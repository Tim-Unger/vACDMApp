using System.Net.Http.Json;
using VACDMApp.Data;
using VACDMApp.Data.Renderer;
using static VACDMApp.VACDMData.VACDMData;
using static VACDMApp.Windows.Views.LoadingView;

namespace VACDMApp.VACDMData
{
    public class FlowMeasuresData
    {
        public static async Task<(List<FlowMeasure> FlowMeasures, List<Fir> Firs)> GetFlowMeasuresAsync()
        {
            Data.LoadingView.SetLabelText(LoadingStatus.FlowMeasures);

            //TODO
            var firs = await GetFirsAsync();

            var measures = await Client.GetFromJsonAsync<List<FlowMeasure>>(
                "https://ecfmp.vatsim.net/api/v1/flow-measure"
            );

            measures.ForEach(
                x =>
                    x.NotifiedFirs = x.notifiedFirs.Select(y => firs.First(z => z.Id == y)).ToList()
            );

            measures.ForEach(
                x =>
                {
                    (x.Measure.MeasureType, x.Measure.MeasureTypeString) = GetMeasureType(x.Measure.TypeRaw);
                }
            );

            measures
                .Where(x => x.WithdrawnAt is not null)
                .ToList()
                .ForEach(x => x.IsWithdrawn = true);

            measures.ForEach(x => x.MeasureStatus = FlowMeasures.GetStatus(x).Status);

            var sortedMeasures = measures.OrderBy(x => x.MeasureStatus).ToList();

            return (sortedMeasures, firs);
        }

        //TODO
        private static async Task<List<Fir>> GetFirsAsync() =>
            await Client.GetFromJsonAsync<List<Fir>>(
                "https://ecfmp.vatsim.net/api/v1/flight-information-region"
            );

        private static (MeasureType MeasureType, string MeasureTypeString) GetMeasureType(string measureTypeRaw) =>
            measureTypeRaw switch
            {
                "minimum_departure_interval" => (MeasureType.MDI, "MDI"),
                "average_departure_interval" => (MeasureType.ADI, "ADI"),
                "per_hour" => (MeasureType.FlightsPerHour, "Flights per Hour"),
                "miles_in_trail" => (MeasureType.MIT, "Miles in Trail"),
                "max_ias" => (MeasureType.MaxIas, "Max Indicated Airspeed"),
                "max_mach" => (MeasureType.MaxMach, "Max Mach Number"),
                "ias_reduction" => (MeasureType.IasReduction, "Reduce IAS by"),
                "mach_reduction" => (MeasureType.MachReduction, "Reduce Mach Number by"),
                "prohibit" => (MeasureType.Prohibit, "Prohibit"),
                "ground_stop" => (MeasureType.GroundStop, "Ground Stop"),
                "mandatory_route" => (MeasureType.MandatoryRoute, "Mandatory Route"),
                _ => throw new ArgumentOutOfRangeException(nameof(measureTypeRaw))
            };
    }
}
