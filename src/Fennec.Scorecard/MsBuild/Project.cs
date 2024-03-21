namespace Fennec.Scorecard.MsBuild;

public class Project : MsBuildParsedItem
{
    public string Name { get; internal set; }
    public List<PackageReference> Packages { get; internal set; } = new();
   
}