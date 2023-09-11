using Flurl.Http;
using ContributorsFacesGenerator.Models.Client;
using ContributorsFacesGenerator.Models.Common;

namespace ContributorsFacesGenerator.Services
{
    public class GithubClient
    {
        private readonly string? m_githubToken;

        public GithubClient(AppConfig appConfig)
        {
            m_githubToken = appConfig.GithubToken;
        }

        public async Task<List<GithubContributor>> GetContributors(string repo, int page = 1)
        {
            var api = $"https://api.github.com/repos/{repo}/contributors?per_page=100&page={page}";
            var request = api.WithHeaders(StaticResources.Headers);
            if (m_githubToken != null)
            {
                request = request.WithHeader("Authorization", $"basic {m_githubToken}");
            }
            var response = await request
                .GetAsync();
            var result = await response.GetJsonAsync<List<GithubContributor>>();

            return result;
        }
    }
}
