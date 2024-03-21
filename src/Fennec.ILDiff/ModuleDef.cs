namespace Fennec.ILDiff;

public class ModuleDef 
{
    private readonly string _name;
    private readonly IReadOnlyCollection<TypeDef> _types;
    private readonly bool _main;
        
    public IReadOnlyCollection<TypeDef> Types => _types;
    public bool Main => _main;
    public string Name => _name;

    internal ModuleDef(string name, List<TypeDef> types, bool main)
    {
        _name = name;
        _types = types;
        _main = main;
    }
}