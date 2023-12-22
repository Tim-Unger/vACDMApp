using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightPositionGrid(VACDMPilot pilot, Vacdm vacdm)
        {
            var flightPositionGrid = new Grid()
            {
                Padding = 10,
                Margin = 10,
                BackgroundColor = _darkBlue
            };
            flightPositionGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );

            var flightPositionFirstGrid = FlightPositionFirstRowGrid(vacdm);
            flightPositionGrid.Children.Add(flightPositionFirstGrid);
            flightPositionGrid.SetRow(flightPositionFirstGrid, 0);

            var flightPositionSecondGrid = FlightPositionSecondRowGrid(pilot, vacdm);
            flightPositionGrid.Children.Add(flightPositionSecondGrid);
            flightPositionGrid.SetRow(flightPositionSecondGrid, 1);

            var flightPositionThirdGrid = FlightPositionThirdRowGrid(pilot);
            flightPositionGrid.Children.Add(flightPositionThirdGrid);
            flightPositionGrid.SetRow(flightPositionThirdGrid, 2);

            return flightPositionGrid;
        }
    }
}
