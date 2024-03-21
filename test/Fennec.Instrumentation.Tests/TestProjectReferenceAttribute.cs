using System;

namespace Fennec.Instrumentation.Tests
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class TestProjectReferenceAttribute : Attribute
    {
        public TestProjectReferenceAttribute(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; }
        public string Path { get; }
    }
}
