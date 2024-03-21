using System.Collections.Specialized;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.XPath;
using ListDiff;

namespace Fennec.ILDiff;

public static class CompareExtensions
{
    /// <summary>
    /// Logic that will return if assembly on the `left` is same as `right`. 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>AssemblyCompareResult with details</returns>
    public static AssemblyCompareResult CompareTo(this AssemblyDef left, AssemblyDef right)
    {
        var result = new AssemblyCompareResult();

        if (left.IsExactlyEqualTo(right))
        {
            return AssemblyCompareResult.AreTheSame(); //Files are _exactly_ the same we can stop here.
        }
        
        //Limitation, now we only compare Main modules. 
        left.Main.CompareModule(right.Main, result);
        
        return result;
    }

    private static void CompareModule(this ModuleDef source, ModuleDef destination, AssemblyCompareResult result)
    {
        var dff = new ListDiff<TypeDef, TypeDef>(source.Types, destination.Types,
            (leftType, rightType) => leftType.FqName == rightType.FqName);

        //Compare the types and handle the differences
        foreach (var act in dff.Actions)
        {
            switch(act.ActionType)
            {
                case ListDiffActionType.Update:
                    //Types are the same we should compare it's contents
                    act.SourceItem.CompareType(act.DestinationItem, result);
                    break;
                case ListDiffActionType.Add:
                    result.AddResult($"Add Type '{act.DestinationItem.FqName}'");
                    break;
                case ListDiffActionType.Remove:
                    result.AddResult($"Remove Type '{act.SourceItem.FqName}'");
                    break;
                default:    
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private static void CompareType(this TypeDef source, TypeDef destination, AssemblyCompareResult result)
    {
        var methods = new ListDiff<MethodDef, MethodDef>(source.Methods, destination.Methods,
            (leftType, rightType) => leftType.Name == rightType.Name);

        foreach (var act in methods.Actions)
        {
            switch (act.ActionType)
            {
                case ListDiffActionType.Update:
                    //Types are the same we should compare it's contents
                    act.SourceItem.CompareInstructions(act.DestinationItem, result);
                    break;
                case ListDiffActionType.Add:
                    result.AddResult($"Add Method '{act.DestinationItem.Name}'");
                    break;
                case ListDiffActionType.Remove:
                    result.AddResult($"Remove Method '{act.SourceItem.Name}'");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static void CompareInstructions(this MethodDef source, MethodDef destination, AssemblyCompareResult result)
    {
        var instructions = new ListDiff<string, string>(source.Instructions, destination.Instructions,
            (left, right) => left == right);
        
        foreach (var act in instructions.Actions)
        {
            switch (act.ActionType)
            {
                case ListDiffActionType.Update:
                    //Types are the same we should compare it's contents
                    if (act.SourceItem!=act.DestinationItem)
                        result.AddResult($"Diff Instruction '{act.DestinationItem}' in '{destination.Name}'");
                    break;
                case ListDiffActionType.Add:
                    result.AddResult($"Add Instruction '{act.DestinationItem}' to '{source.Name}'");
                    break;
                case ListDiffActionType.Remove:
                    result.AddResult($"Remove Instruction '{act.SourceItem}' from '{source.Name}'");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}