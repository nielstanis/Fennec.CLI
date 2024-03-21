using Fennec.Instrumentation.Result;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Fennec.Instrumentation
{
    public class AssemblyAnalyzer
    {
        private readonly string _assembly;

        public AssemblyAnalyzer(string assembly)
        {
            _assembly = assembly;
        }

        public AssemblyResult Analyse()
        {
            try
            {
                using (var ass = AssemblyDefinition.ReadAssembly(_assembly))
                {
                    var assembly = new AssemblyResult(ass.FullName, _assembly);

                    foreach (var module in ass.Modules.OrderBy(m => m.FileName))
                    foreach (var classType in module.GetTypes().OrderBy(z => z.FullName).Where(z => !z.IsInterface))
                    {
                        var typeResult = new ClassTypeResult(classType.FullName, classType.Module.FileName);
                        foreach (var method in classType.Methods.Where(e => !e.IsAbstract).OrderBy(e => e.FullName))
                        {
                            int seq = 0;
                            if (method.HasBody)
                            {
                                var parameters = string.Join(",", method.Parameters);
                                var methodResult = new MethodResult(method.Name, parameters);                       
                                
                                foreach (var instruction in method.Body.Instructions
                                    .Where(u => ((u.OpCode == OpCodes.Call)) 
                                    || (u.OpCode == OpCodes.Callvirt) 
                                    || (u.OpCode == OpCodes.Calli)
                                    || (u.OpCode == OpCodes.Newobj)
                                    ))
                                {
                                    var splits = instruction.Operand.ToString().Split(" ");
                                    var invoke = new InvocationResult(splits[1], splits[0], seq++);
                                    methodResult.Invocations.Add(invoke);
                                }
                                typeResult.Methods.Add(methodResult);
                            }
                        }
                        assembly.Types.Add(typeResult);
                    }
                    return assembly;
                }
            }
            catch (Exception ex)
            {
                var err = new AssemblyResult("NotAvailable", _assembly);
                err.HandleException(ex);
                return err;
            }
        }
    }
}