using ContributorsFacesGenerator.Extensions;
using ContributorsFacesGenerator.Models.Client;
using RazorEngineCore;
using SkiaSharp;
using System.Text;
using ContributorsFacesGenerator.Models.Common;

namespace ContributorsFacesGenerator.Services
{
    public class GithubService
    {
        private readonly GithubClient m_githubClient;

        public GithubService(GithubClient githubClient)
        {
            m_githubClient = githubClient;
        }

        public async Task<byte[]> GetContributorsFaces(string repo, int width = 860, OutputMode mode = OutputMode.Png)
        {
            var contributors = new List<GithubContributor>();
            var tempContributors = new List<GithubContributor>();
            do
            {
                tempContributors = await m_githubClient.GetContributors(repo);
                contributors.AddRange(tempContributors);
            } while (tempContributors.Count == 100);
            tempContributors.Clear();

            byte[] data;
            if (mode == OutputMode.Png)
                data = await CreateFacesImage(contributors, width);
            else data = await CreateFacesHtml(contributors, width);
            return data;
        }

        private async Task<byte[]> CreateFacesHtml(List<GithubContributor> contributors, int width)
        {
            var templateText = Encoding.UTF8.GetString(Assets.ContributorsFaces);
            IRazorEngine razorEngine = new RazorEngine();

            var template = await razorEngine.CompileAsync(templateText);

            var result = await template.RunAsync(new
            {
                Contributors = contributors,
                Width = width,
            });

            return Encoding.UTF8.GetBytes(result);
        }

        private async Task<byte[]> CreateFacesImage(List<GithubContributor> contributors, int width)
        {
            var elementWidth = 50;
            var elementHeight = 50;
            var imgWidth = 46;
            var margin = (elementWidth - imgWidth) / 2;
            var aLineElementCount = GetALineElementCount(width, elementWidth);
            var lineCount = contributors.Count / aLineElementCount;
            if (contributors.Count % aLineElementCount > 0) lineCount++;
            var height = elementHeight * lineCount;

            using var bitMap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitMap);
            using var paint = new SKPaint { Color = SKColors.White, IsAntialias = true };
            canvas.Clear(SKColors.Transparent);
            var currentLine = 0;
            var currentIndex = 0;

            using var path = new SKPath();
            foreach (var githubContributor in contributors)
            {
                path.AddOval(new SKRect(margin + currentIndex * elementWidth, margin + currentLine * elementHeight, margin + currentIndex * elementWidth + imgWidth, margin + currentLine * elementHeight + imgWidth));
                currentIndex++;
                if (currentIndex < aLineElementCount) continue;
                currentIndex = 0;
                currentLine++;
            }

            canvas.ClipPath(path);
            currentLine = 0;
            currentIndex = 0;

            foreach (var githubContributor in contributors)
            {
                var img = await CustomImageExtensions.CreateSkImg(githubContributor.AvatarUrl);
                using var bitmap = SKBitmap.FromImage(img);
                canvas.DrawBitmap(bitmap, new SKRect(margin + currentIndex * elementWidth, margin + currentLine * elementHeight, margin + currentIndex * elementWidth + imgWidth, margin + currentLine * elementHeight + imgWidth), paint);
                canvas.Save();
                currentIndex++;
                if (currentIndex < aLineElementCount) continue;
                currentIndex = 0;
                currentLine++;
            }
            using var image = SKImage.FromBitmap(bitMap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.ToArray();
        }

        private int GetALineElementCount(int lineWidth, int elementWidth)
        {
            if (lineWidth < elementWidth) return 0;
            return lineWidth / elementWidth;
        }
    }
}
