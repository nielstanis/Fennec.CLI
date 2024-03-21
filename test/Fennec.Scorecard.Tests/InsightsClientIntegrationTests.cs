using Fennec.Scorecard.DepsDev;
using Xunit;

namespace Fennec.Scorecard.Tests;

public class InsightsClientIntegrationTests
{

    [Fact]
    public async void GetInsightsForClient()
    {
        var tempFile = Path.GetTempFileName();
        var client = new InsightsClient();
        var res = await client.TryGetInsightsAsync("newtonsoft.json", "13.0.2", tempFile);
    }
}