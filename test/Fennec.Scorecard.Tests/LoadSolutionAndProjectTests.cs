using Fennec.Scorecard.MsBuild;
using Xunit;

namespace Fennec.Scorecard.Tests;

public class LoadSolutionAndProjectTests
{

    [Fact]
    public void LoadFennecProjectRelativePath()
    {
        var project = Path.GetFullPath("../../../../../src/Fennec/Fennec.csproj");
        bool result = Parser.TryReadPackagesFromProject(project, out Project proj);
        
        Assert.Equal(project, proj.Name);
        Assert.True(result);
        Assert.False(proj.HadErrorLoading);
        
        Assert.Single(proj.Packages);
        var packageRef = proj.Packages.First();
        Assert.Equal("McMaster.Extensions.CommandLineUtils", packageRef.Name);
        Assert.Equal("4.1.0", packageRef.Version);
    }
    
    [Fact]
    public void LoadDasBlogSolution()
    {
        bool result = Parser.TryReadPackagesFromSolution(@"/Users/nelson/github/dasblog-core/source/DasBlog All.sln", out Solution sln);
        Assert.Equal(10, sln.Projects.Count);
        Assert.False(sln.HadErrorLoading);
    }
    
    [Fact]
    public void LoadAppInspectorSolution()
    {
        bool result = Parser.TryReadPackagesFromSolution(@"/Users/nelson/github/ApplicationInspector/AppInspector.sln", out Solution sln);
        Assert.Equal(8, sln.Projects.Count);
        Assert.False(sln.HadErrorLoading);
    }
}