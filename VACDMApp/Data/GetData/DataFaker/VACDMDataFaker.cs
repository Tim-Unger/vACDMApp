using Bogus;
using VACDMApp;
using VACDMApp.VACDMData;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.DataFaker
{
    public class VACDMFaker
    {
        public static List<VACDMPilot> FakePilots()
        {
            var positionFaker = new Faker<Position>()
                .RuleFor(x => x.Latitude, y => float.Parse(y.Address.Latitude().ToString()))
                .RuleFor(x => x.Longitude, y => float.Parse(y.Address.Longitude().ToString()));

            var now = DateTime.UtcNow;

            var vacdmFaker = new Faker<Vacdm>()
                .RuleFor(x => x.Eobt, y => y.Date.Soon())
                .RuleFor(x => x.Tobt, y => y.Date.Soon())
                .RuleFor(x => x.Exot, y => y.Random.Int(0, 20))
                .RuleFor(x => x.Tsat, y => y.Date.Soon())
                .RuleFor(x => x.Ctot, y => y.Date.Soon())
                .RuleFor(x => x.Ttot, y => y.Date.Soon())
                .RuleFor(x => x.TaxiZone, y => "Bogus Gate")
                .RuleFor(x => x.IsTaxizoneTaxiout, y => false);

            var airports = new string[] { "EDDF", "EDDL", "EDDK", "EDDS", "EDDM", "EDDH", "EDDB" };

            var flightPlanFaker = new Faker<FlightPlan>()
                .RuleFor(x => x.FlightRules, y => "I")
                .RuleFor(x => x.Departure, y => y.Random.ArrayElement(airports))
                .RuleFor(x => x.Arrival, y => y.Random.ArrayElement(airports));

            var runways = new string[] { "07", "06", "25L", "25R", "16", "34R", "03" };

            var clearanceFaker = new Faker<Clearance>()
                .RuleFor(x => x.DepRwy, y => y.Random.ArrayElement(runways))
                .RuleFor(x => x.Sid, y => $"{y.Random.Utf16String(5)}{y.Random.Int(0, 9)}{y.Random.Utf16String(1)}")
                .RuleFor(x => x.InitialClimb, y => "5000")
                .RuleFor(x => x.AssignedSquawk, y => y.Random.Int(2001, 2110).ToString())
                .RuleFor(x => x.CurrentSquawk, y => "1000");

            var airlines = new string[] { "DLH", "AUA", "BOX", "BAW", "RYR", "LHA", "EWG" };

            var pilotFaker = new Faker<VACDMPilot>()
                .RuleFor(x => x.Position, y => positionFaker.Generate())
                .RuleFor(x => x.Vacdm, y => vacdmFaker.Generate())
                .RuleFor(x => x.FlightPlan, y => flightPlanFaker.Generate())
                .RuleFor(x => x.Clearance, y => clearanceFaker.Generate())
                .RuleFor(x => x.Id, y => y.Random.Int().ToString())
                .RuleFor(x => x.Callsign, y => "DLH123")
                .RuleFor(x => x.HasBooking, y => false)
                .RuleFor(x => x.IsInactive, y => false)
                .RuleFor(x => x.Measures, y => Array.Empty<object>())
                .RuleFor(x => x.CreatedAt, y => y.Date.Recent())
                .RuleFor(x => x.UpdatedAt, y => y.Date.Recent())
                .RuleFor(x => x.V, y => 1);

            return pilotFaker.Generate(10);
        }
    }
}
