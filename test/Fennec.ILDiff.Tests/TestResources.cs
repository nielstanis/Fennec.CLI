using System.Reflection;

namespace Fennec.ILDiff.Tests
{
    public class TestResources
    {
        public static string GetTestProjectAssembly(string name)
        {
            return typeof(TestResources)
                .Assembly
                .GetCustomAttributes<TestProjectReferenceAttribute>()
                .First(a => a.Name == name)
                .Path;
        }
    }
}
