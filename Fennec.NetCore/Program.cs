using System;
using McMaster.Extensions.CommandLineUtils;

namespace Fennec.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.ReadLine();

            var app = new CommandLineApplication(throwOnUnexpectedArg:false);
            app.Description = "Dump used API's of .NET assembly/assemblies.";
            app.AddName("Fennec.NetCore");
            app.FullName = "Fennec.NetCore - .NET Core API dumper";
            app.HelpOption("-h|--help");
            var outputFolder = app.Option("-o|--output <FOLDER>", "Output Folder", CommandOptionType.SingleOrNoValue);

            app.OnExecute(() =>
            {
                foreach (var arg in app.RemainingArguments)
                {
                    if (System.IO.File.Exists(arg))
                    {
                        var loadedAssembly = new AnalyseAssembly(arg, outputFolder.HasValue() ? outputFolder.Value() : null);
                        loadedAssembly.Analyse();
                    }
                    else
                    {
                        var files = System.IO.Directory.GetFiles(AppContext.BaseDirectory, arg);
                        foreach (var file in files)
                        {
                            var loadedAssembly = new AnalyseAssembly(file, outputFolder.HasValue() ? outputFolder.Value() : null);
                            loadedAssembly.Analyse();
                        }
                    }
                }
            });

            app.Execute(args);
        }
    }
}