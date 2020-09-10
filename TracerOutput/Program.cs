using System;
using TracerLib;
namespace TracerOutput
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();
            OutputResultToConsole outputResultToConsole = new OutputResultToConsole();
            TracerTest tester = new TracerTest(tracer);

            tester.TestMethod();
            outputResultToConsole.OutputResult(tracer);


        }
    }
}
