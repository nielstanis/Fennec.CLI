using Xunit;

namespace Fennec.ILDiff.Tests;

public class CreateAssemblyDefTests
{

    [Fact]
    public void BasicConsoleTest()
    {
        var x = TestResources.GetTestProjectAssembly("BasicConsole");
        var assemblyDef = AssemblyDef.Create(x);

        //We have two types, module and Program
        Assert.Equal(2, assemblyDef.Types.Count());
        //Single Module
        Assert.True(assemblyDef.Modules.Any());

        //Iterate over types ordered in sequence it's created (might want to do ordering future)
        using var types  = assemblyDef.Types.GetEnumerator();
        
        types.MoveNext();
        Assert.Equal("<Module>", types.Current.FqName);

        types.MoveNext();
        Assert.Equal("BasicConsole.Program", types.Current.FqName);
        Assert.Equal(3,types.Current.Methods.Count());

        using var method = types.Current.Methods.GetEnumerator();
        method.MoveNext();
        Assert.Equal("System.Void BasicConsole.Program::Main(System.String[])", method.Current.Name);
        Assert.Equal(37, method.Current.Instructions.Count);
        Assert.Equal("IL_0000: nop",method.Current.Instructions.First());
        Assert.Equal("IL_0029: ldloc.1",method.Current.Instructions.Skip(20).First());
        Assert.Equal("IL_0051: ret",method.Current.Instructions.Last());
        
        method.MoveNext();
        Assert.Equal("System.Boolean BasicConsole.Program::DoOperation(System.Int32,System.String)", method.Current.Name);
        Assert.Equal(11, method.Current.Instructions.Count);
        var expectedInstructions = new[]
        {
            "IL_0000: nop"
            ,"IL_0001: ldarg.1"
            ,"IL_0002: ldc.i4.s 30"
            ,"IL_0004: mul"
            ,"IL_0005: stloc.0"
            ,"IL_0006: ldarg.2"
            ,"IL_0007: call System.Boolean System.IO.File::Exists(System.String)"
            ,"IL_000c: stloc.1"
            ,"IL_000d: br.s IL_000f"
            ,"IL_000f: ldloc.1"
            ,"IL_0010: ret"
        };
        Assert.Equal(expectedInstructions,method.Current.Instructions);
        
        method.MoveNext();
        Assert.Equal("System.Void BasicConsole.Program::.ctor()", method.Current.Name);

        Assert.False(method.MoveNext()); //No methods left
        
        //Nothing left in type enumerator
        Assert.False(types.MoveNext());
    }

    [Fact]
    public void Library1PersonTest()
    {
        var x = TestResources.GetTestProjectAssembly("LibraryPerson1");
        var assemblyDef = AssemblyDef.Create(x);

        //We have 2 types, 1 module and class Person
        Assert.Equal(2, assemblyDef.Types.Count());
        
        //Focus on Person for further testing
        var personType = assemblyDef.Types.FirstOrDefault(x => x.FqName == "LibraryPerson1.Person");
        Assert.NotNull(personType);
        
        Assert.Equal(1, personType.Events.Count());
        Assert.Equal("System.EventHandler LibraryPerson1.Person::PersonChanged",personType.Events.First());
        
        Assert.Equal(4, personType.Fields.Count());
        using var field = personType.Fields.GetEnumerator();
        field.MoveNext();
        Assert.Equal("System.DateOnly LibraryPerson1.Person::_birthDay", field.Current);
        field.MoveNext();
        Assert.Equal("System.String LibraryPerson1.Person::_name", field.Current);
        field.MoveNext();
        Assert.Equal("System.String LibraryPerson1.Person::_surname", field.Current);
        field.MoveNext();
        Assert.Equal("System.EventHandler LibraryPerson1.Person::PersonChanged", field.Current);
        Assert.False(field.MoveNext());
        
        Assert.Equal(2, personType.Properties.Count());
        using var property = personType.Properties.GetEnumerator();
        property.MoveNext();
        Assert.Equal("System.String LibraryPerson1.Person::Name()", property.Current);
        property.MoveNext();
        Assert.Equal("System.String LibraryPerson1.Person::Surname()", property.Current);
        Assert.False(property.MoveNext());
        
        Assert.Equal(9,personType.Methods.Count());
        var method = personType.Methods.FirstOrDefault(m => m.Name == "System.Void LibraryPerson1.Person::Method()");
        Assert.NotNull(method);
        Assert.Equal(13,method.Instructions.Count());
        var expectedInstructions = new[]
        {
            "IL_0000: nop"
            ,"IL_0001: ldarg.0"
            ,"IL_0002: ldftn System.String LibraryPerson1.Person::<Method>b__13_0(System.String)"
            ,"IL_0008: newobj System.Void System.Func`2<System.String,System.String>::.ctor(System.Object,System.IntPtr)"
            ,"IL_000d: stloc.0"
            ,"IL_000e: ldstr \"Result \""
            ,"IL_0013: ldloc.0"
            ,"IL_0014: ldstr \"/etc/hosts\""
            ,"IL_0019: callvirt !1 System.Func`2<System.String,System.String>::Invoke(!0)"
            ,"IL_001e: call System.String System.String::Concat(System.String,System.String)"
            ,"IL_0023: call System.Void System.Console::WriteLine(System.String)"
            ,"IL_0028: nop"
            ,"IL_0029: ret"
        };
        Assert.Equal(expectedInstructions, method.Instructions);
    }
}