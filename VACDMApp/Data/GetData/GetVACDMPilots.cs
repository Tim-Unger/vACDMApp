using static VACDMApp.VACDMData.VACDMData;
using static VACDMApp.VACDMData.Data;
using VACDMApp.DataFaker;

namespace VACDMApp.VACDMData
{
    internal class VACDMPilotsData
    {
        private static readonly string TestData = @"[
  {
    ""position"": {
      ""lat"": 51.28105,
      ""lon"": 6.76726
    },
    ""vacdm"": {
      ""eobt"": ""2023-11-18T17:20:00.000Z"",
      ""tobt"": ""2023-11-18T17:20:00.000Z"",
      ""tobt_state"": ""GUESS"",
      ""exot"": 11,
      ""manual_exot"": false,
      ""tsat"": ""2023-11-18T17:20:00.000Z"",
      ""ctot"": ""1969-12-31T23:59:59.999Z"",
      ""ttot"": ""2023-11-18T17:31:00.000Z"",
      ""asrt"": ""1969-12-31T23:59:59.999Z"",
      ""aort"": ""1969-12-31T23:59:59.999Z"",
      ""asat"": ""1969-12-31T23:59:59.999Z"",
      ""aobt"": ""1969-12-31T23:59:59.999Z"",
      ""delay"": 0,
      ""prio"": 0,
      ""sug"": ""1969-12-31T23:59:59.999Z"",
      ""pbg"": ""1969-12-31T23:59:59.999Z"",
      ""txg"": ""1969-12-31T23:59:59.999Z"",
      ""taxizone"": ""A01 - A09"",
      ""taxizoneIsTaxiout"": false,
      ""blockAssignment"": ""2023-11-18T16:27:13.450Z"",
      ""blockId"": 105,
      ""block_rwy_designator"": ""23L""
    },
    ""flightplan"": {
      ""flight_rules"": ""I"",
      ""departure"": ""EDDL"",
      ""arrival"": ""LOWW""
    },
    ""clearance"": {
      ""dep_rwy"": ""23L"",
      ""sid"": ""DODEN9T"",
      ""initial_climb"": ""5000"",
      ""assigned_squawk"": ""1000"",
      ""current_squawk"": ""1000""
    },
    ""_id"": ""6558e5e12761e6c40dbad43d"",
    ""callsign"": ""AAL211"",
    ""hasBooking"": false,
    ""inactive"": false,
    ""measures"": [
      
    ],
    ""createdAt"": ""2023-11-18T16:27:13.513Z"",
    ""updatedAt"": ""2023-11-18T16:51:59.477Z"",
    ""__v"": 0
  },
  {
    ""position"": {
      ""lat"": 51.29014000000001,
      ""lon"": 6.77417
    },
    ""vacdm"": {
      ""eobt"": ""2023-11-18T16:50:00.000Z"",
      ""tobt"": ""2023-11-18T16:50:00.000Z"",
      ""tobt_state"": ""GUESS"",
      ""exot"": 10,
      ""manual_exot"": false,
      ""tsat"": ""2023-11-18T16:50:00.000Z"",
      ""ctot"": ""1969-12-31T23:59:59.999Z"",
      ""ttot"": ""2023-11-18T17:00:00.000Z"",
      ""asrt"": ""1969-12-31T23:59:59.999Z"",
      ""aort"": ""1969-12-31T23:59:59.999Z"",
      ""asat"": ""1969-12-31T23:59:59.999Z"",
      ""aobt"": ""1969-12-31T23:59:59.999Z"",
      ""delay"": 0,
      ""prio"": 0,
      ""sug"": ""1969-12-31T23:59:59.999Z"",
      ""pbg"": ""1969-12-31T23:59:59.999Z"",
      ""txg"": ""1969-12-31T23:59:59.999Z"",
      ""taxizone"": ""default taxitime"",
      ""taxizoneIsTaxiout"": false,
      ""blockAssignment"": ""2023-11-18T16:27:18.585Z"",
      ""blockId"": 102,
      ""block_rwy_designator"": ""23L""
    },
    ""flightplan"": {
      ""flight_rules"": ""I"",
      ""departure"": ""EDDL"",
      ""arrival"": ""EGKK""
    },
    ""clearance"": {
      ""dep_rwy"": ""23L"",
      ""sid"": ""SONEB7T"",
      ""initial_climb"": ""5000"",
      ""assigned_squawk"": ""2033"",
      ""current_squawk"": ""2033""
    },
    ""_id"": ""6558e5e62761e6c40dbad45d"",
    ""callsign"": ""KLM862"",
    ""hasBooking"": false,
    ""inactive"": false,
    ""measures"": [
      
    ],
    ""createdAt"": ""2023-11-18T16:27:18.650Z"",
    ""updatedAt"": ""2023-11-18T16:53:31.654Z"",
    ""__v"": 0
  },
  {
    ""position"": {
      ""lat"": 51.28193,
      ""lon"": 6.765529999999999
    },
    ""vacdm"": {
      ""eobt"": ""2023-11-18T00:00:00.000Z"",
      ""tobt"": ""2023-11-18T16:52:24.264Z"",
      ""tobt_state"": ""CONFIRMED"",
      ""exot"": 11,
      ""manual_exot"": false,
      ""tsat"": ""2023-11-18T16:52:24.264Z"",
      ""ctot"": ""1969-12-31T23:59:59.999Z"",
      ""ttot"": ""2023-11-18T17:03:24.264Z"",
      ""asrt"": ""2023-11-18T16:52:30.654Z"",
      ""aort"": ""1969-12-31T23:59:59.999Z"",
      ""asat"": ""2023-11-18T16:52:30.596Z"",
      ""aobt"": ""1969-12-31T23:59:59.999Z"",
      ""delay"": 0,
      ""prio"": 3,
      ""sug"": ""1969-12-31T23:59:59.999Z"",
      ""pbg"": ""1969-12-31T23:59:59.999Z"",
      ""txg"": ""1969-12-31T23:59:59.999Z"",
      ""taxizone"": ""A10 - B04"",
      ""taxizoneIsTaxiout"": false,
      ""blockAssignment"": ""2023-11-18T16:31:02.390Z"",
      ""blockId"": 102,
      ""block_rwy_designator"": ""23L""
    },
    ""flightplan"": {
      ""flight_rules"": ""I"",
      ""departure"": ""EDDL"",
      ""arrival"": ""LOWW""
    },
    ""clearance"": {
      ""dep_rwy"": ""23L"",
      ""sid"": ""DODEN9T"",
      ""initial_climb"": ""5000"",
      ""assigned_squawk"": ""1000"",
      ""current_squawk"": ""1000""
    },
    ""_id"": ""6558e6c62761e6c40dbad7c8"",
    ""callsign"": ""DLH462"",
    ""hasBooking"": false,
    ""inactive"": false,
    ""measures"": [
      
    ],
    ""createdAt"": ""2023-11-18T16:31:02.479Z"",
    ""updatedAt"": ""2023-11-18T16:52:25.049Z"",
    ""__v"": 0
  },
  {
    ""position"": {
      ""lat"": 51.28036,
      ""lon"": 6.76479
    },
    ""vacdm"": {
      ""eobt"": ""2023-11-18T17:20:00.000Z"",
      ""tobt"": ""2023-11-18T16:50:51.929Z"",
      ""tobt_state"": ""CONFIRMED"",
      ""exot"": 11,
      ""manual_exot"": false,
      ""tsat"": ""2023-11-18T16:50:51.929Z"",
      ""ctot"": ""1969-12-31T23:59:59.999Z"",
      ""ttot"": ""2023-11-18T17:01:51.929Z"",
      ""asrt"": ""2023-11-18T16:50:54.776Z"",
      ""aort"": ""1969-12-31T23:59:59.999Z"",
      ""asat"": ""2023-11-18T16:50:54.717Z"",
      ""aobt"": ""2023-11-18T16:53:51.584Z"",
      ""delay"": 0,
      ""prio"": 3,
      ""sug"": ""1969-12-31T23:59:59.999Z"",
      ""pbg"": ""1969-12-31T23:59:59.999Z"",
      ""txg"": ""1969-12-31T23:59:59.999Z"",
      ""taxizone"": ""A10 - B04"",
      ""taxizoneIsTaxiout"": false,
      ""blockAssignment"": ""2023-11-18T16:33:35.413Z"",
      ""blockId"": 102,
      ""block_rwy_designator"": ""23L""
    },
    ""flightplan"": {
      ""flight_rules"": ""I"",
      ""departure"": ""EDDL"",
      ""arrival"": ""LSZH""
    },
    ""clearance"": {
      ""dep_rwy"": ""23L"",
      ""sid"": ""DODEN9T"",
      ""initial_climb"": ""5000"",
      ""assigned_squawk"": ""1000"",
      ""current_squawk"": ""1000""
    },
    ""_id"": ""6558e75f2761e6c40dbada3f"",
    ""callsign"": ""DLH511"",
    ""hasBooking"": false,
    ""inactive"": false,
    ""measures"": [
      
    ],
    ""createdAt"": ""2023-11-18T16:33:35.476Z"",
    ""updatedAt"": ""2023-11-18T16:54:58.500Z"",
    ""__v"": 0
  }
]";

        internal static async Task<List<VACDMPilot>> GetVACDMPilotsAsync()
        {
            var data = await Client.GetStringAsync(VacdmApiUrl);

            var dataList = JsonSerializer.Deserialize<List<VACDMPilot>>(data);

            if (dataList.Count == 0)
            {
//#if DEBUG
//                return VACDMFaker.FakePilots();
//#endif
                return Enumerable.Empty<VACDMPilot>().ToList();
                ////TODO
                //dataList = JsonSerializer.Deserialize<List<VACDMPilot>>(TestData);

                //var random = new Random();

                //dataList.ForEach(x => x.Callsign = VACDMPilots[random.Next(0, VACDMPilots.Count)].Callsign);
            }

            //Remove VFR Flights
            return dataList.Where(x => x.FlightPlan.FlightRules == "I")
                    .OrderBy(x => x.Vacdm.Eobt)
                    .ToList();
        }
    }
}
