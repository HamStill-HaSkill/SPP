using System;
using System.Collections.Generic;
using System.Text;

namespace TracerLib
{
    public class ThreadResult
    {
        public int Id { get; set; }
        public long Time { get; set; }
        public List<TraceResult> Methods { get; set; }
        public ThreadResult()
        {
            Methods = new List<TraceResult>();
            Time = 0;
        }
    }
}
