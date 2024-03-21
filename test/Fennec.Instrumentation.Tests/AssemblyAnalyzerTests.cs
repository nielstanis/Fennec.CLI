
using System.Linq;
using Fennec.Instrumentation;
using Fennec.Instrumentation.Result;
using Xunit;

namespace Fennec.Instrumentation.Tests
{
    public class AssemblyAnalyzerTests
    {
        [Fact]
        public void BasicConsoleResultTest()
        {
            var x = TestResources.GetTestProjectAssembly("BasicConsole");
            var result = new AssemblyAnalyzer(x);
            AssemblyResult assemblyResult = result.Analyse();
            Assert.Equal(2, assemblyResult.Types.Count);
            Assert.Equal("<Module>", assemblyResult.Types[0].ClassType);
            Assert.Equal("BasicConsole.Program", assemblyResult.Types[1].ClassType);
            
            //Focus on 2nd classtype
            var ct = assemblyResult.Types[1];
            Assert.Equal(3, ct.Methods.Count());

            //Focus on 3rd method
            var mt = ct.Methods[2];
            Assert.Equal("Main", mt.Name);
            Assert.Equal("args", mt.Parameters);
            
            Assert.Equal(7, mt.Invocations.Count);
        }
    }
}
