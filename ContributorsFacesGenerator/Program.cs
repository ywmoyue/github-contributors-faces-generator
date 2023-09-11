using System.Text;
using ContributorsFacesGenerator.Extensions;
using ContributorsFacesGenerator.Models.Common;
using ContributorsFacesGenerator.Services;
//dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true
var config = InitArgs(args);
if (string.IsNullOrEmpty(config.Repo))
{
    throw new Exception("need repo url");
}
config.OutputFilePath.CheckFilePath();

var client = new GithubClient(config);
var githubService = new GithubService(client);
var data = await githubService.GetContributorsFaces(config.Repo, config.Width, config.Mode);
var file = new FileInfo(config.OutputFilePath);
if (config.Mode == OutputMode.Png)
{
    using var stream = file.Create();
    await stream.WriteAsync(data);
}
else
{
    using var stream = file.CreateText();
    var text = Encoding.UTF8.GetString(data);
    await stream.WriteAsync(text);
}

Console.WriteLine("Success!");

AppConfig InitArgs(string[] args)
{
    var repo = ParseArg("-r", "--repo");
    var width = ParseArg("-w", "--width");
    var token = ParseArg("-s", "--secret");
    var output = ParseArg("-o", "--output");
    var mode = ParseArg("-m", "--mode");
    var outputMode = OutputMode.Png;
    var defaultExt = "png";
    if (mode is "html")
    {
        outputMode = OutputMode.Html;
        defaultExt = "html";
    }
    return new AppConfig()
    {
        Repo = repo,
        Width = width != null ? int.Parse(width) : 860,
        GithubToken = token,
        OutputFilePath = output ?? $"./output.{defaultExt}",
        Mode = outputMode,
    };
}

string? ParseArg(string shortCommand, string command)
{
    var argIndex = Array.IndexOf(args, shortCommand);
    if (argIndex < 0)
    {
        argIndex = Array.IndexOf(args, command);
    }

    if (argIndex < 0) return null;
    if (args.Length <= argIndex + 1)
    {
        throw new Exception("args not match");
    }
    var arg = args[argIndex + 1];
    return arg;
}