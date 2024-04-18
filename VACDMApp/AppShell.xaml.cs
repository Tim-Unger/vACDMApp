using CommunityToolkit.Maui.Alerts;
using VacdmApp.Data;
using VacdmApp.Windows.BottomSheets;
using VacdmApp.Windows.Popups;

namespace VacdmApp
{
    public partial class AppShell : Shell
    {
        private static int _backSwipeCount = 0;

        public AppShell()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            var sender = Data.Data.SenderPage;

            if (sender == SenderPage.SingleFlight)
            {
                ((SingleFlightBottomSheet)Data.Data.Sender).DismissAsync();
                Data.Data.Sender = Data.Data.FlightsView;
                Data.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.Vdgs)
            {
                ((VDGSBottomSheet)Data.Data.Sender).DismissAsync();
                Data.Data.Sender = Data.Data.FlightsView;
                Data.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.Airport)
            {
                ((AirportsBottomSheet)Data.Data.Sender).DismissAsync();
                Data.Data.Sender = Data.Data.FlightsView;
                Data.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.About)
            {
                Current.GoToAsync("..", true);
                Data.Data.Sender = Data.Data.FlightsView;
                Data.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.Time)
            {
                ((TimesBottomSheet)Data.Data.Sender).DismissAsync();
                Data.Data.Sender = Data.Data.FlightsView;
                Data.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if(sender == SenderPage.FirSettings)
            {
                ((FirPopup)Data.Data.Sender).CloseAsync();
                Data.Data.Sender = Data.Data.FlightsView;
                Data.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (_backSwipeCount == 0)
            {
                var toast = Toast.Make(
                    "Back again to exit",
                    CommunityToolkit.Maui.Core.ToastDuration.Short,
                    14
                );
                toast.Show().ConfigureAwait(false);

                _backSwipeCount++;

                return true;
            }

            if (_backSwipeCount == 1)
            {
                _backSwipeCount = 0;
                Data.Data.SenderPage = SenderPage.Default;
                return false;
            }

            _backSwipeCount = 0;
            return true;
        }
    }
}
