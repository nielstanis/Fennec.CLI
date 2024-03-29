using System.Text.Json.Serialization;

namespace Fennec.Instrumentation.Result
{
    public class AssemblyResult
    {
        private readonly string _assembly;
        private readonly string _filePath;
        public string Assembly { get { return _assembly; } }
        [JsonIgnore]
        public string FilePath { get { return _filePath; }}
        public AssemblyResult(string assembly, string filePath)
        {
            _assembly = assembly;
            _filePath = filePath;
            Types = new List<ClassTypeResult>();
        }

        public List<ClassTypeResult> Types {get; private set;}

        [JsonIgnore]
        public Exception ExceptionOccurred { get; private set;} 
        public void HandleException(Exception ex)
        {
            ExceptionOccurred = ex;
        }
        [JsonIgnore]
        public bool HasError
        {
            get
            {
                return ExceptionOccurred != null;
            }
        }
    }
}