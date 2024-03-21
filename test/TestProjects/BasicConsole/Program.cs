using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace BasicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var taint = args[0];
            var otherstring = "test";

            if (System.IO.File.Exists(taint))
            {
                var x = System.IO.File.OpenRead(taint);
            }

            if (System.IO.File.Exists(otherstring))
            {
                var u = System.IO.File.OpenRead(otherstring);
#pragma warning disable SYSLIB0011
                var bin = new BinaryFormatter();
#pragma warning restore SYSLIB0011
                var instance = bin.Deserialize(u);
            }
        }

        public bool DoOperation(int input, string otherInput)
        {
            int data = input * 30;
            return System.IO.File.Exists(otherInput);
        }
    }
}
