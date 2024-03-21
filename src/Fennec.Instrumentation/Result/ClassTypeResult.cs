namespace Fennec.Instrumentation.Result
{
    public class ClassTypeResult
    {
        private readonly string _classtype;
        private readonly string _module;

        public ClassTypeResult(string classtype, string module)
        {
            Methods = new List<MethodResult>();
            
            _classtype = classtype;
            _module = module;
        }
        public List<MethodResult> Methods {get; private set;}
        public string ClassType {get { return _classtype; }}
    }
}