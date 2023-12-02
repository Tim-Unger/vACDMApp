namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static void BookmarkButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var parentGrid = (Grid)((Grid)button.Parent).Parent;
            var callsignGrid = (Grid)parentGrid.Children[1];
            var callsignLabel = (Label)callsignGrid.Children[1];
            var callsign = callsignLabel.Text;

            var pilot = VACDMData.Data.VACDMPilots.First(x => x.Callsign == callsign);

            if (VACDMData.Data.BookmarkedPilots.Contains(pilot))
            {
                button.Source = "bookmark_outline.svg";
                VACDMData.Data.BookmarkedPilots.Remove(pilot);
                return;
            }

            button.Source = "bookmark.svg";
            VACDMData.Data.BookmarkedPilots.Add(pilot);
        }
    }
}
