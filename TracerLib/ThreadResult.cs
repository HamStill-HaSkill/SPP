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
        public ThreadResult(int id)
        {
            Id = id;
            Methods = new List<TraceResult>();
            Time = 0;
        }
    }
}
