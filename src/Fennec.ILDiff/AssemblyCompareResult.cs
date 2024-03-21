namespace Fennec.ILDiff;

public class AssemblyCompareResult
{
    public bool AreExactlyTheSame { get; private set; }
    private List<string> _res = new();
    public IReadOnlyCollection<string> Items => _res.AsReadOnly();

    internal static AssemblyCompareResult AreTheSame()
    {
        return new AssemblyCompareResult
        {
            AreExactlyTheSame = true
        };
    }

    internal AssemblyCompareResult()
    {
        AreExactlyTheSame = false;
    }

    internal void AddResult(string message)
    {
        _res.Add(message);
    }
    
}