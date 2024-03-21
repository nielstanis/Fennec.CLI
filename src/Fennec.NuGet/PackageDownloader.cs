namespace Fennec.NuGet;

public class PackageDownloader
{
    private readonly HttpClient _client;

    public PackageDownloader(HttpClient client)
    {
        _client = client;
    }

    public PackageDownloader() : this(new HttpClient())
    {

    }
}