using System;
using System.IO;
using System.Linq;
using Mono.Cecil.Cil;

namespace Fennec.NetCore
{
    public class AnalyseAssembly
    {
        private readonly string _assembly;
        private readonly string _folder;
        private readonly string _outputFile;

        public AnalyseAssembly(string assembly, string folder = null)
        {
            _assembly = assembly;
            _folder = folder;

            if (!string.IsNullOrEmpty(folder)) 
            {
                if (!System.IO.Directory.Exists(folder)) 
                    System.IO.Directory.CreateDirectory(folder);
            }
            else
            {
                _folder = System.IO.Path.GetDirectoryName(assembly);
            }

            string filename = System.IO.Path.GetFileNameWithoutExtension(assembly);
            _outputFile = System.IO.Path.Combine(_folder, $"{filename}.fxt");
        }


        public bool Analyse()
        {
            bool result = true;

            try
            {
                using (var ass = Mono.Cecil.AssemblyDefinition.ReadAssembly(_assembly))
                using (var writer = System.IO.File.CreateText(_outputFile))
                {
                    foreach (var module in ass.Modules.OrderBy(m => m.FileName))
                    foreach (var classType in module.GetTypes().OrderBy(z => z.FullName).Where(z => !z.IsInterface))
                    {
                        foreach (var method in classType.Methods.Where(e => !e.IsAbstract).OrderBy(e => e.FullName))
                        {
                            if (method.HasBody)
                            {
                                //Construct list of parameters used.
                                string parameters = String.Join(",", method.Parameters);

                                foreach (var instruction in method.Body.Instructions
                                    .Where(u => ((u.OpCode == OpCodes.Call)) 
                                    || (u.OpCode == OpCodes.Callvirt) 
                                    || (u.OpCode == OpCodes.Calli)
                                    || (u.OpCode == OpCodes.Newobj)
                                    ))
                                {
                                    //Split out typecall from methodbody and construct output. 
                                    var typecall = instruction.Operand.ToString().Split(" ")[1];
                                    writer.WriteLine($"{classType}::{method.Name}({parameters})::{typecall}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred when processing {_assembly}");
                Console.WriteLine($"Stack:");
                Console.WriteLine(ex.ToString());
                result = false;
            }
            return result;
        }
    }
}