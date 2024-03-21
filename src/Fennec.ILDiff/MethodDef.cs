namespace Fennec.ILDiff;

public class MethodDef {
    private readonly string _name;
    private readonly IReadOnlyCollection<string> _instructions;
    public string Name => _name;
    public IReadOnlyCollection<string> Instructions => _instructions;

    internal MethodDef(string name, IReadOnlyCollection<string> instructions)
    {
        _name = name;
        _instructions = instructions;
    }
}