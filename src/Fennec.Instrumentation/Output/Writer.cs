using Fennec.Instrumentation.Result;

namespace Fennec.Instrumentation.Output
{
    public abstract class Writer
    {
        protected readonly string _outputFolder;

        public Writer(string outputFolder)
        {
            _outputFolder = outputFolder;
        }

        public bool EnsureFolderCreated()     
        {
            var result = Directory.Exists(_outputFolder);
            if (!result)
            {
                var x = Directory.CreateDirectory(_outputFolder);
                result = Directory.Exists(_outputFolder);
            }
            return result;
        }

        public abstract Task<bool> WriteOutputAsync(AssemblyResult assemblyResult);
    }
}