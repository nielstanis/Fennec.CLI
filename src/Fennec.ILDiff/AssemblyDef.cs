

using System.Reflection;
using System.Security.Cryptography;

namespace Fennec.ILDiff;

/// <summary>
/// Assembly definition with corresponding modules and types.
/// </summary>
public class AssemblyDef 
{
    private readonly string _file;
    private readonly string _sha256;
    private readonly string _name;
    private readonly IReadOnlyCollection<ModuleDef> _modules;
    private readonly ModuleDef _main;

    public IEnumerable<TypeDef> Types => _main.Types;
    public IEnumerable<ModuleDef> Modules => _modules;
    public string File => _file;
    public string Name => _name;
    public string Sha256 => _sha256;
    public ModuleDef Main => _main;

    private AssemblyDef(string file, string sha256, string name, IReadOnlyCollection<ModuleDef> modules)
    {
        ArgumentException.ThrowIfNullOrEmpty(file);
        
        _file = file;
        _sha256 = sha256;
        _name = name;
        _modules = modules;
        _main = _modules.First(x => x.Main); //There should always be a single main module. 
    }

    /// <summary>
    /// Compare to other AssemblyDef and return if its _exactly_ equal (sha256 hash + name). 
    /// </summary>
    /// <param name="toCompare"></param>
    /// <returns></returns>
    public bool IsExactlyEqualTo(AssemblyDef toCompare)
    {
        return this.Name == toCompare.Name 
               && this.Sha256 == toCompare.Sha256; 
    }

    /// <summary>
    /// Heuristically it might only be enough to index methods and their definitions.
    /// Fields, Properties, and Events are also included.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static AssemblyDef Create(string file)
    {
        var modules = new List<ModuleDef>();
        var ass = Mono.Cecil.AssemblyDefinition.ReadAssembly(file);
        string sha256 = GetChecksum(file);
        foreach (var md in ass.Modules)
        {
            var types = new List<TypeDef>();
            foreach (var td in md.Types)
            {
                var fields = td.Fields.Select(f => f.FullName).ToList();
                var properties = td.Properties.Select(f => f.FullName).ToList();
                var events = td.Events.Select(f => f.FullName).ToList();
                var methods = td.Methods.Select(method => new MethodDef(method.FullName, method.Body.Instructions.Select(inst => inst.ToString()).ToList())).ToList();
                types.Add(new TypeDef(td.Namespace, td.Name, methods, fields, properties, events));
            }

            modules.Add(new ModuleDef(md.Name, types, md.IsMain));
        }

        return new AssemblyDef(file, sha256, ass.FullName, modules);
    }
    
    /// <summary>
    /// Generates SHA256 hash of file it's contents 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static string GetChecksum(string file)
    {
        using var stream = System.IO.File.OpenRead(file);
        var sha = SHA256.Create();
        var checksum = sha.ComputeHash(stream);
        return BitConverter.ToString(checksum).Replace("-", String.Empty);
    }
}