using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Label ConfirmationStatusLabel(Vacdm vacdm)
        {
            (var confirmationStatusText, var confirmationColor) =
                vacdm.TobtState == "CONFIRMED"
                    ? ("CONFIRMED", Colors.LimeGreen)
                    : ("UNCONFIRMED", Colors.Red);
            var confirmationStatusLabel = new Label()
            {
                Text = confirmationStatusText,
                TextColor = confirmationColor,
                Background = Colors.Transparent,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            return confirmationStatusLabel;
        }
    }
}
