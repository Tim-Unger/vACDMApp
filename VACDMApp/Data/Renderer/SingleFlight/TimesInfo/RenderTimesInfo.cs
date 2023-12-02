using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid TimesInfoGrid(VACDMPilot pilot)
        {
            var timesInfoGrid = new Grid()
            {
                Padding = 10,
                Margin = 10,
                BackgroundColor = _darkBlue
            };
            timesInfoGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesInfoGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesInfoGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );

            var vacdm = pilot.Vacdm;

            var timesFirstColumnGrid = FlightInfoFirstRowGrid(vacdm);
            timesInfoGrid.Children.Add(timesFirstColumnGrid);
            timesInfoGrid.SetRow(timesFirstColumnGrid, 0);

            var timesSecondRowGrid = FlightInfoSecondRowGrid(vacdm);
            timesInfoGrid.Children.Add(timesSecondRowGrid);
            timesInfoGrid.SetRow(timesSecondRowGrid, 1);

            var confirmationStatusLabel = ConfirmationStatusLabel(vacdm);
            timesInfoGrid.Children.Add(confirmationStatusLabel);
            timesInfoGrid.SetRow(confirmationStatusLabel, 3);

            return timesInfoGrid;
        }
    }
}
