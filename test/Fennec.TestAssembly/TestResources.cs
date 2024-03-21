using System.Reflection;

namespace Fennec.TestAssembly
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
