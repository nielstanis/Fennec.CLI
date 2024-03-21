using Xunit;

namespace Fennec.ILDiff.Tests;

public class CompareAssemblyDefTests
{

    [Fact]
    public void BasicConsoleCompareToItSelfTest()
    {
        var left = AssemblyDef.Create(TestResources.GetTestProjectAssembly("BasicConsole"));
        var right = AssemblyDef.Create(TestResources.GetTestProjectAssembly("BasicConsole"));
        var compare = left.CompareTo(right);
        Assert.True(compare.AreExactlyTheSame);
        Assert.Equal(left.Sha256, right.Sha256);
        Assert.Equal(left.Name, right.Name);
        Assert.Equal(0, compare.Items.Count);
    }
    
    [Fact]
    public void BasicConsoleCompareToLibraryPerson1()
    {
        var left = AssemblyDef.Create(TestResources.GetTestProjectAssembly("BasicConsole"));
        var right = AssemblyDef.Create(TestResources.GetTestProjectAssembly("LibraryPerson1"));
        var compare = left.CompareTo(right);
        Assert.False(compare.AreExactlyTheSame);
        Assert.NotEqual(left.Sha256, right.Sha256);
        Assert.NotEqual(left.Name, right.Name);
        var items = new[]
        {
             "Remove Type 'BasicConsole.Program'", "Add Type 'LibraryPerson1.Person'"
        };
        Assert.Equal(items, compare.Items);
    }
    
    [Fact]
    public void LibraryPerson1CompareToLibraryPerson2()
    {
        var left = AssemblyDef.Create(TestResources.GetTestProjectAssembly("LibraryPerson1"));
        var right = AssemblyDef.Create(TestResources.GetTestProjectAssembly("LibraryPerson2"));
        var compare = left.CompareTo(right);
        Assert.False(compare.AreExactlyTheSame);
        Assert.NotEqual(left.Sha256, right.Sha256);
        Assert.NotEqual(left.Name, right.Name);
        var items = new[]
        {
            "Remove Instruction 'IL_0027: ret' from 'System.Void LibraryPerson1.Person::OnPersonChanged()'"
            ,"Add Instruction 'IL_0027: ldstr \"Additional invoke\"' to 'System.Void LibraryPerson1.Person::OnPersonChanged()'"
            ,"Add Instruction 'IL_002c: call System.Void System.Console::WriteLine(System.String)' to 'System.Void LibraryPerson1.Person::OnPersonChanged()'"
            ,"Add Instruction 'IL_0031: nop' to 'System.Void LibraryPerson1.Person::OnPersonChanged()'"
            ,"Add Instruction 'IL_0032: ret' to 'System.Void LibraryPerson1.Person::OnPersonChanged()'"
            ,"Add Method 'System.Void LibraryPerson1.Person::Missing()'"
        };
        Assert.Equal(items, compare.Items);
    }
    
    [Fact]
    public void LibraryPerson1ComparedToCopiedVersion()
    {
        var fileLocation = new FileInfo(TestResources.GetTestProjectAssembly("LibraryPerson1"));
        Assert.NotNull(fileLocation);
        var otherLocation = Path.Combine(fileLocation.DirectoryName, "test.dll");
        if (File.Exists(otherLocation)) File.Delete(otherLocation);
        File.Copy(fileLocation.FullName, otherLocation);
        
        var left = AssemblyDef.Create(fileLocation.FullName);
        var right = AssemblyDef.Create(otherLocation);
        var compare = left.CompareTo(right);
        Assert.True(compare.AreExactlyTheSame);
        Assert.Equal(left.Sha256, right.Sha256);
        Assert.Equal(left.Name, right.Name);
        Assert.Equal(0, compare.Items.Count);
    }
}