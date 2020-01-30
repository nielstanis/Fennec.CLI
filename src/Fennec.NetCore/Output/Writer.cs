using System.Threading.Tasks;
using Fennec.NetCore.Result;

namespace Fennec.NetCore.Output
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
            var result = System.IO.Directory.Exists(_outputFolder);
            if (!result)
            {
                var x = System.IO.Directory.CreateDirectory(_outputFolder);
                result = System.IO.Directory.Exists(_outputFolder);
            }
            return result;
        }

        public abstract Task<bool> WriteOutputAsync(AssemblyResult assemblyResult);
    }
}