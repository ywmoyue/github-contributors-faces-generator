namespace ContributorsFacesGenerator.Models.Common
{
    public static class StaticResources
    {
        public static Dictionary<string, string> Headers = new Dictionary<string, string>()
        {
            {"Accept-Encoding", "gzip, deflate, br"},
            {"Accept", "*/*"},
            {"Connection", "keep-alive"},
            {
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36"
            },
            {"Cache-Control", "no-cache"},
        };
    }
}
