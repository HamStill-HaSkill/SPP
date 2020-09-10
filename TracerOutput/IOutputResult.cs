using System;
using System.Collections.Generic;
using System.Text;
using TracerLib;

namespace TracerOutput
{
    interface IOutputResult
    {
        void OutputResult(ITracer tracer);
    }
}
