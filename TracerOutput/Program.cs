using Newtonsoft.Json;
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
            tester.TestTestTset();

            var testThread = new Thread(() =>
            {
                tester.TestTestTset();
            });
            testThread.Start();
            testThread.Join();
            
            SerializeToJSON serializerJSON = new SerializeToJSON();
            SerializeToXML serializerXML = new SerializeToXML();
            OutputResultToFile outputResultToFile = new OutputResultToFile();
            outputResultToConsole.OutputResult(serializerJSON.Serialize(tracer));
            outputResultToConsole.OutputResult(serializerXML.Serialize(tracer));
            outputResultToFile.SavePath = "C:\\Users\\Xiaomi\\source\\repos\\TracerLib\\SPP\\Files\\JSON.json";
            outputResultToFile.OutputResult(serializerJSON.Serialize(tracer));
            outputResultToFile.SavePath = "C:\\Users\\Xiaomi\\source\\repos\\TracerLib\\SPP\\Files\\XML.xml";
            outputResultToFile.OutputResult(serializerXML.Serialize(tracer));

        }
    }
}
