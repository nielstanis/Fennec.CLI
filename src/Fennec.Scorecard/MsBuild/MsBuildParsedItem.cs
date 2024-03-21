namespace Fennec.Scorecard.MsBuild;

public class MsBuildParsedItem
{
    public bool HadErrorLoading { get; internal set; }
    public string Error { get; internal set; } = string.Empty;
}