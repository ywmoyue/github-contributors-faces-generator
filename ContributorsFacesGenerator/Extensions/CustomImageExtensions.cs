using ContributorsFacesGenerator.Models.Common;
using Flurl.Http;
using SkiaSharp;

namespace ContributorsFacesGenerator.Extensions
{
    public static class CustomImageExtensions
    {
        public static async Task<SKImage> CreateSkImg(string url)
        {
            using var stream = await url.WithHeaders(StaticResources.Headers).GetStreamAsync();
            var img = SKImage.FromEncodedData(stream);
            return img;
        }
    }
}
