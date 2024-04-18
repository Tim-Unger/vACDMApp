namespace VacdmApp.Data
{
    public class VacdmPilotsData
    {
        public static async Task<List<VacdmPilot>> GetVacdmPilotsAsync()
        {
            if(VacdmData.VacdmApiUrl is null)
            {
                return Enumerable.Empty<VacdmPilot>().ToList();
            }

            var data = await VacdmData.Client.GetStringAsync(VacdmData.VacdmApiUrl);

            var dataList = JsonSerializer.Deserialize<List<VacdmPilot>>(data);

            if (dataList.Count == 0)
            {
                return Enumerable.Empty<VacdmPilot>().ToList();
            }

            //Remove VFR Flights
            return dataList
                .Where(x => x.FlightPlan.FlightRules == "I")
                .OrderBy(x => x.Vacdm.Eobt)
                .ToList();
        }
    }
}
