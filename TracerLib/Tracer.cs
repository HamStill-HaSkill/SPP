using System;

namespace TracerLib
{
    public class Tracer : ITracer
    {
        private long startTime = 0;
        private long stopTime = 0;
        public void StartTrace()
        {
            startTime = DateTime.Now.Ticks;

        }
        public void StopTrace()
        {
            stopTime = DateTime.Now.Ticks;
        }
        public TraceResult TraceResult()
        {
            TraceResult result = new TraceResult();
            result.time = stopTime - startTime;
            return result;
        }
    }
}
