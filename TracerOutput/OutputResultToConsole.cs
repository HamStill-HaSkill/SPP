using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using TracerLib;


namespace TracerOutput
{
    class OutputResultToConsole : IOutputResult
    {
        public void OutputResult(ITracer tracer)
        {
            SerializeToJSON serializer = new SerializeToJSON();
            
            Console.WriteLine(serializer.Serialize(tracer));

        }
    }
}
