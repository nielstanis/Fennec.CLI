namespace Fennec.NetCore.Output
{
    public static class WriterFactory
    {
        public static Writer CreateWriter(string writerType, string output)
        {
            Writer res = new FxtWriter(output);
            if (writerType.ToLower().Trim()=="json")
            {
                res = new JsonWriter(output);
            }
            return res;
        }
    }
}