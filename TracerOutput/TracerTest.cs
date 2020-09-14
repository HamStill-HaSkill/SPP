using System;
using System.Collections.Generic;
using System.Text;
using TracerLib;
using System.Threading;

namespace TracerOutput
{
    class TracerTest
    {
        private ITracer _tracer;
        internal TracerTest(ITracer tracer)
        {
            _tracer = tracer;
        }
        public void TestMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(1000);
            _tracer.StopTrace();
        }
    }
}
