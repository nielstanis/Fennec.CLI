namespace Fennec.Scorecard.MsBuild;

public class Solution : MsBuildParsedItem
{
    public string? Name { get; internal set; }
    public List<Project> Projects { get; internal set; } = new();
}