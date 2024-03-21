
using System.Runtime.CompilerServices;

namespace Fennec.Scorecard.MsBuild;

public static class Parser
{
    public static bool TryReadPackagesFromSolution(string? sln, out Solution result)
    {
        result = new Solution
        {
            Name = sln
        };
        
        try
        {
            var readSolution = ByteDev.DotNet.Solution.DotNetSolution.Load(sln);
            foreach (var proj in readSolution.Projects)
            {
                var project = new Project();
                
                string osPath = proj.Path;
                if (Path.DirectorySeparatorChar == '/')
                    osPath = proj.Path.Replace('\\', '/');
                var absolutPathProject = Path.Combine(Path.GetDirectoryName(sln) ?? "./", osPath); 
                
                project.HadErrorLoading = !TryReadPackagesFromProject(absolutPathProject, out project);
                result.Projects.Add(project);
            } 
            result.HadErrorLoading = false;
        }
        catch (Exception e)
        {
            result.HadErrorLoading = true;
            result.Error = e.ToString();
        }
        
        return !result.HadErrorLoading;
    }

    /// <summary>
    /// Will try to parse given project `project` and return a `Fennec.ScoreCard.MsBuild.Project` containing the packagereferences used. 
    /// </summary>
    /// <param name="project">csproj location</param>
    /// <param name="result">Project structure</param>
    /// <returns>Project and Packages used</returns>
    public static bool TryReadPackagesFromProject(string project, out Project result)
    {
        result = new Project();
        try
        {
            result.Name = project;
            var readProject = ByteDev.DotNet.Project.DotNetProject.Load(result.Name);
            foreach (var package in readProject.PackageReferences)
            {
                result.Packages.Add(new PackageReference { Name = package.Name, Version = package.Version });
            }

            result.HadErrorLoading = false;
        }
        catch (Exception e)
        {
            result.HadErrorLoading = true;
            result.Error = e.ToString();
        }
        return !result.HadErrorLoading;
    }
}