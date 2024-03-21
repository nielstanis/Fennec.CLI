using System.Text.Json;
using Fennec.Instrumentation.Result;

namespace Fennec.Instrumentation.Output
{
    public class JsonWriter : Writer
    {
        public JsonWriter(string outputFolder) : base(outputFolder)
        {
        }

        public override async Task<bool> WriteOutputAsync(AssemblyResult assemblyResult)
        {
            string filename = Path.GetFileNameWithoutExtension(assemblyResult.FilePath);
            string outputFile = Path.Combine(_outputFolder, $"{filename}.json");

            bool result = true;
            try
            {
                EnsureFolderCreated();
                using (var f = File.Create(outputFile))
                {
                    await JsonSerializer.SerializeAsync(f, assemblyResult);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}