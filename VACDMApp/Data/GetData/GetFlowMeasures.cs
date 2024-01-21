using System.Net.Http.Json;
using VACDMApp.Data;
using VACDMApp.Data.Renderer;
using static VACDMApp.VACDMData.VACDMData;
using static VACDMApp.Windows.Views.LoadingView;

namespace VACDMApp.VACDMData
{
    public class FlowMeasuresData
    {
        public static async Task<List<FlowMeasure>> GetFlowMeasuresAsync()
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
                    x.Measure.MeasureType = GetMeasureType(x.Measure.TypeRaw);
                    x.Measure.MeasureTypeString = GetMeasureTypeString(x.Measure.TypeRaw);
                }
            );

            measures
                .Where(x => x.WithdrawnAt is not null)
                .ToList()
                .ForEach(x => x.IsWithdrawn = true);

            measures.ForEach(x => x.MeasureStatus = FlowMeasures.GetStatus(x).Status);

            return measures.OrderBy(x => x.MeasureStatus).ToList();
        }

        //TODO
        private static async Task<List<Fir>> GetFirsAsync() =>
            await Client.GetFromJsonAsync<List<Fir>>(
                "https://ecfmp.vatsim.net/api/v1/flight-information-region"
            );

        private static MeasureType GetMeasureType(string measureTypeRaw) =>
            measureTypeRaw switch
            {
                "minimum_departure_interval" => MeasureType.MDI,
                "average_departure_interval" => MeasureType.ADI,
                "per_hour" => MeasureType.FlightsPerHour,
                "miles_in_trail" => MeasureType.MIT,
                "max_ias" => MeasureType.MaxIas,
                "max_mach" => MeasureType.MaxMach,
                "ias_reduction" => MeasureType.IasReduction,
                "mach_reduction" => MeasureType.MachReduction,
                "prohibit" => MeasureType.Prohibit,
                "ground_stop" => MeasureType.GroundStop,
                "mandatory_route" => MeasureType.MandatoryRoute,
                _ => throw new ArgumentOutOfRangeException(nameof(measureTypeRaw))
            };

        private static string GetMeasureTypeString(string measureTypeRaw) =>
            measureTypeRaw switch
            {
                "minimum_departure_interval" => "MDI",
                "average_departure_interval" => "ADI",
                "per_hour" => "Flights per Hour",
                "miles_in_trail" => "Miles in Trail",
                "max_ias" => "Max Indicated Airspeed",
                "max_mach" => "Max Mach Number",
                "ias_reduction" => "Reduce IAS by",
                "mach_reduction" => "Reduce Mach Number by",
                "prohibit" => "Prohibit",
                "ground_stop" => "Ground Stop",
                "mandatory_route" => "Mandatory Route",
                _ => throw new ArgumentOutOfRangeException(nameof(measureTypeRaw))
            };
    }
}
