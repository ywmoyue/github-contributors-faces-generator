namespace ContributorsFacesGenerator.Models.Common
{
    public class AppConfig
    {
        public string? GithubToken { get; set; }

        public string? Repo { get; set; }

        public int Width { get; set; }

        public string OutputFilePath { get; set; }

        public OutputMode Mode { get; set; }
    }
}
