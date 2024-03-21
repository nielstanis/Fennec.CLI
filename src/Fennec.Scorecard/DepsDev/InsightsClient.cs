using System.Net.Http.Json;
using System.Web;

namespace Fennec.Scorecard.DepsDev;

public class InsightsClient
{
    private readonly HttpClient _client;
    private const string InsightsUrl = @"https://api.deps.dev/v3alpha/projects/{0}";
    private const string SourceUrl = @"https://api.deps.dev/v3alpha/systems/nuget/packages/{0}/versions/{1}";

    public InsightsClient() : this(new HttpClient())
    {
        //Intentionally left blank :) 
    }

    public InsightsClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<bool> TryGetInsightsAsync(string package, string version, string writeToPath)
    {
        bool result = false;
        try
        {
            //First we need to determine repo url based on given package version.
            var root = await _client.GetFromJsonAsync<Root>(string.Format(SourceUrl, package, version));
            if (root != null)
            {
                //Get package details based on source url 
                var sourceRepo = root?.links.FirstOrDefault(x => x.label == "SOURCE_REPO");
                if (sourceRepo != null)
                {
                    var trimmedUrl = sourceRepo.url.Substring(8).ToLower(); //get rid of 'https://'
                    var url = string.Format(InsightsUrl, HttpUtility.UrlEncode(trimmedUrl));
                    using var fileStream = File.OpenWrite(writeToPath);
                    _client.GetStreamAsync(url).Result.CopyTo(fileStream);
                    result = true; 
                }
            }
        }
        catch (Exception e)
        {
            //Intentionally left blank. 
        }
        
        return result;
    }
}

public class Link
{
    public string label { get; set; }
    public string url { get; set; }
}

public class Root
{
    public VersionKey versionKey { get; set; }
    public bool isDefault { get; set; }
    public List<string> licenses { get; set; }
    public List<object> advisoryKeys { get; set; }
    public List<Link> links { get; set; }
    public List<object> slsaProvenances { get; set; }
    public List<string> registries { get; set; }
    public DateTime publishedAt { get; set; }
}

public class VersionKey
{
    public string system { get; set; }
    public string name { get; set; }
    public string version { get; set; }
}