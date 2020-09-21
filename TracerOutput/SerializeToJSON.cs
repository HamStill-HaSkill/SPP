using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TracerLib;

namespace TracerOutput
{
    public class SerializeToJSON : ISerialize
    {
        public string Serialize(ITracer tracer)
        {
            return JsonConvert.SerializeObject(tracer.TraceResult(), Formatting.Indented);
        }
    }
}
