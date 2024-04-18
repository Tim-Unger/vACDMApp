namespace VacdmApp.Data
{
    public class VacdmPilotsData
    {
        public static async Task<List<VacdmPilot>> GetVacdmPilotsAsync()
        {
            if (VacdmData.VacdmApiUrl is null)
            {
                return Enumerable.Empty<VacdmPilot>().ToList();
            }

            try
            {
                var data = await VacdmData.Client.GetStringAsync(VacdmData.VacdmApiUrl);

                var dataList = JsonSerializer.Deserialize<List<VacdmPilot>>(data);

                if (dataList.Count == 0)
                {
                    return Enumerable.Empty<VacdmPilot>().ToList();
                }

                //Removes VFR Flights 
                return dataList
                    .Where(x => x.FlightPlan.FlightRules == "I")
                    .OrderBy(x => x.Vacdm.Eobt)
                    .ToList();
            }
            //TODO Properly catch exception and show error to the user when the request times out. Currently this exception is thrown after the overlying exception is thrown, breaking the catch block
            catch
            {
                return Enumerable.Empty<VacdmPilot>().ToList();
            }
        }
    }
}
