using Fennec.Instrumentation.Result;

namespace Fennec.Instrumentation.Output
{

    public class FxtWriter : Writer
    {
        public FxtWriter(string outputFolder) : base(outputFolder)
        {
            
        }

        public override async Task<bool> WriteOutputAsync(AssemblyResult assemblyResult)
        {
            string filename = Path.GetFileNameWithoutExtension(assemblyResult.FilePath);
            string outputFile = Path.Combine(_outputFolder, $"{filename}.fxt");

            bool result = true;
            try
            {
                EnsureFolderCreated();
                await using (var f = File.CreateText(outputFile))
                {
                    //for flat file the ordering is important, order by type, methods and sequence of invocation. 
                    foreach (var t in assemblyResult.Types.OrderBy(x => x.ClassType))
                    {
                        foreach (var m in t.Methods.OrderBy(z => z.Name))
                        {
                            foreach (var i in m.Invocations.OrderBy(r => r.Sequence))
                            {
                                await f.WriteLineAsync($"{t.ClassType}::{m.Name}({m.Parameters})::{i.Invocation}");
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

    }
}