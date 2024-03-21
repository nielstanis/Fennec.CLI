namespace Fennec.Instrumentation.Result
{
    public class InvocationResult
    {
        private readonly string _invoke;
        private readonly string _returnType;
        private readonly int _sequence;

        public InvocationResult(string invoke, string returnType, int sequence)
        {
            _returnType = returnType;
            _invoke = invoke;
            _sequence = sequence;
        }

        public int Sequence {get{return _sequence;}}
        public string Invocation {get {return _invoke;}}

        public string ReturnType {get {return _returnType;}}
    }
}