
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fennec.Instrumentation;
using Fennec.Instrumentation.Output;
using Fennec.Scorecard.DepsDev;
using Fennec.Scorecard.MsBuild;
using McMaster.Extensions.CommandLineUtils;

namespace Fennec
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Description = "Fennec CLI - .NET Security Tools";
            app.AddName("Fennec CLI");
            app.FullName = "Fennec CLI - .NET Security Tools";
            app.HelpOption("-h|--help");
            app.VersionOptionFromAssemblyAttributes("-v|--version", typeof(Program).Assembly);
            var cmd = app.Argument("command", "command to execute: 'scorecard'", false);
            cmd.IsRequired();
            
            // var files = app.Option("-a|--assemblies <FILES/PATTERN>", "List of assemblies or file pattern", CommandOptionType.MultipleValue);
            // var outputFolder = app.Option("-o|--output <FOLDER>", "Output Folder", CommandOptionType.SingleOrNoValue);
            // var outputType = app.Option("-f|--format <FORMAT>", "File Format, either JSON or FXT", CommandOptionType.SingleOrNoValue);
            
            app.OnExecuteAsync(async cancellationToken =>
            {
                switch (cmd.Value)
                {
                    case "scorecard":
                        await GetScoreCardCurrentDirAsync();
                        break;
                    // case "instrument":
                    //     await CreateInstrumentationForAssemblies(files, outputFolder, outputType);
                    //     break;
                    default:
                        app.ShowHelp();
                        break;
                }
            });

            return await app.ExecuteAsync(args);
        }

        private static async Task GetScoreCardCurrentDirAsync()
        {
            var insightsClient = new InsightsClient();
            const string outputDir = ".fennec";
            
            var solutionFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln");
            var projectFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj", SearchOption.AllDirectories);
            
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            foreach (var sln in solutionFiles)
            {
                var slnPath = Path.GetFullPath(sln);
                Console.WriteLine("Path:"+slnPath);
                Parser.TryReadPackagesFromSolution(slnPath, out Solution parsedSolution);
                Console.WriteLine("Projects:"+parsedSolution.Projects.Count().ToString());
                foreach (var proj in parsedSolution.Projects)
                {
                    Console.WriteLine("Project:"+proj.Name+proj.HadErrorLoading.ToString());
                    foreach (var p in proj.Packages)
                    {
                        var pathToWriteTo = $"{outputDir}/{p.Name}-{p.Version}-ssc.json";
                        await insightsClient.TryGetInsightsAsync(p.Name, p.Version, pathToWriteTo);
                    }
                }
            }
            
            foreach (var proj in projectFiles)
            {
                Parser.TryReadPackagesFromProject(proj, out Project parsedProject);
                foreach (var p in parsedProject.Packages)
                {
                    var pathToWriteTo = $"{outputDir}/{p.Name}-{p.Version}-ssc.json";
                    await insightsClient.TryGetInsightsAsync(p.Name, p.Version, pathToWriteTo);
                }
            }
        }

        private static async Task CreateInstrumentationForAssemblies(CommandOption ass, CommandOption outputFolder, CommandOption outputType)
        {
            string folder = outputFolder.HasValue() ? outputFolder.Value() : ".";
            string type = outputType.HasValue() ? outputType.Value() : "fxt";
            var writer = WriterFactory.CreateWriter(type, folder);

            foreach (var arg in ass.Values)
            {
                if (System.IO.File.Exists(arg))
                {
                    await AnalyzeAndWrite(writer, arg);
                }
                else
                {
                    var files = System.IO.Directory.GetFiles(AppContext.BaseDirectory, arg);
                    foreach (var file in files)
                    {
                        await AnalyzeAndWrite(writer, file);
                    }
                }
            }
        }
        
        private static async Task AnalyzeAndWrite(Writer writer, string assembly)
        {
            var loadedAssembly = new AssemblyAnalyzer(assembly);
            var ass = loadedAssembly.Analyse();
            if (ass.HasError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to analyse '{0}'", ass.FilePath);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Analyzing and writing output of: {0}", ass.Assembly);
                Console.ResetColor();
                _ = await writer.WriteOutputAsync(ass);
            }
        }
    }
}