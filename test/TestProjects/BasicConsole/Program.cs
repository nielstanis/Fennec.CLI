using System;

namespace BasicConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var taint = args[0];

            if (System.IO.File.Exists(taint))
            {
                var x = System.IO.File.OpenRead(taint);
            }

            if (System.IO.File.Exists("test"))
            {
                var u = System.IO.File.OpenRead("test");
            }
        }
    }
}
