using System;
using System.Collections.Generic;
using System.Text;
using TracerLib;

namespace TracerOutput
{
    interface ISerialize
    {
        string Serialize(ITracer tracer);
        
    }
}
