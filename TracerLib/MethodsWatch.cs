using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TracerLib
{
    class MethodsWatch
    {
        public ConcurrentStack<TraceResult> methods { get; set; }
        public ConcurrentStack<Stopwatch> watches { get; set; }
        public MethodsWatch()
        {
            methods = new ConcurrentStack<TraceResult>();
            watches = new ConcurrentStack<Stopwatch>();
        }

    }
}
