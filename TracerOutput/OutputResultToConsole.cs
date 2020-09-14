﻿using System;
using System.Collections.Generic;
using System.Text;
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
