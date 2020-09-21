using System;
using System.Threading;
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
            
            var testThread = new Thread(() => {
                tester.TestTestTset();
            });
            testThread.Start();
            testThread.Join();
            outputResultToConsole.OutputResult(tracer);
        }
    }
}
