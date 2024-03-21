namespace Fennec.ILDiff;

public class TypeDef 
{
    private readonly string _ns;
    private readonly string _name;

    public string FqName => string.IsNullOrEmpty(_ns) ? _name : $"{_ns}.{_name}";

    public IReadOnlyCollection<MethodDef> Methods { get; }

    public IReadOnlyCollection<string> Fields { get; }

    public IReadOnlyCollection<string> Properties { get; }

    public IReadOnlyCollection<string> Events { get; }

    internal TypeDef(string ns, string name, 
        IReadOnlyCollection<MethodDef> methods, 
        IReadOnlyCollection<string> fields,  
        IReadOnlyCollection<string> properties, 
        IReadOnlyCollection<string> events)
    {
        _ns = ns;
        _name = name;
        Methods = methods;
        Fields = fields;
        Properties = properties;
        Events = events;
    }
}