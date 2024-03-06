using CommunityToolkit.Maui.Alerts;
using VACDMApp.VACDMData;
using VACDMApp.Windows.BottomSheets;

namespace VACDMApp
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
            var sender = VACDMData.Data.SenderPage;

            if (sender == SenderPage.SingleFlight)
            {
                ((SingleFlightBottomSheet)VACDMData.Data.Sender).DismissAsync();
                VACDMData.Data.Sender = VACDMData.Data.FlightsView;
                VACDMData.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.Vdgs)
            {
                ((VDGSBottomSheet)VACDMData.Data.Sender).DismissAsync();
                VACDMData.Data.Sender = VACDMData.Data.FlightsView;
                VACDMData.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.Airport)
            {
                ((AirportsBottomSheet)VACDMData.Data.Sender).DismissAsync();
                VACDMData.Data.Sender = VACDMData.Data.FlightsView;
                VACDMData.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.About)
            {
                Current.GoToAsync("..", true);
                VACDMData.Data.Sender = VACDMData.Data.FlightsView;
                VACDMData.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if (sender == SenderPage.Time)
            {
                ((TimesBottomSheet)VACDMData.Data.Sender).DismissAsync();
                VACDMData.Data.Sender = VACDMData.Data.FlightsView;
                VACDMData.Data.SenderPage = SenderPage.Default;
                return true;
            }

            if(sender == SenderPage.FirSettings)
            {
                ((FirBottomSheet)VACDMData.Data.Sender).CloseAsync();
                VACDMData.Data.Sender = VACDMData.Data.FlightsView;
                VACDMData.Data.SenderPage = SenderPage.Default;
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
                VACDMData.Data.SenderPage = SenderPage.Default;
                return false;
            }

            _backSwipeCount = 0;
            return true;
        }
    }
}
