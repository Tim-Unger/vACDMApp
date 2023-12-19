namespace VACDMApp.Data
{
    public static class ExtensionMethods
    {
        public static bool IsTsatInTheWindow(this DateTime tsat)
        {
            var now = DateTime.UtcNow;

            if (tsat.AddMinutes(-5) <= now && tsat.AddMinutes(5) >= now)
            {
                return true;
            }

            return false;
        }

        public static bool IsValidCid(this int cid)
        {
            //CID cannot be smaller than 800_000 (lowest founder CID is 800_006)
            if (cid < 800000)
            {
                return false;
            }

            //CID is an older CID and therefore has one less digit (determined by the first letter)
            if (new[] { 8, 9 }.Any(x => x == int.Parse(cid.ToString()[0].ToString())))
            {
                if (cid.ToString().Length != 6)
                {
                    return false;
                }

                return true;
            }

            //Newer CIDs must be 7 letters long (for now)
            if (cid.ToString().Length != 7)
            {
                return false;
            }

            return true;
        }
    }
}
