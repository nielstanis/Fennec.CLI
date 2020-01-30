using System;
using System.Linq;
using System.Threading.Tasks;
using Fennec.NetCore.Result;

namespace Fennec.NetCore.Output
{

    public class FxtWriter : Writer
    {
        public FxtWriter(string outputFolder) : base(outputFolder)
        {
        }

        public override async Task<bool> WriteOutputAsync(AssemblyResult assemblyResult)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(assemblyResult.FilePath);
            string outputFile = System.IO.Path.Combine(_outputFolder, $"{filename}.fxt");

            bool result = true;
            try
            {
                base.EnsureFolderCreated();
                using (var f = System.IO.File.CreateText(outputFile))
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