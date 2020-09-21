using System.Collections.Generic;
using Newtonsoft.Json;

namespace TracerLib
{
    public class TraceResult
    {
        public long Time { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        [JsonIgnore]
        public int ThreadId { get; set; }
        [JsonIgnore]
        public int Methodlevel { get; set; }
        public List<TraceResult> Methods {get; set; }
        public TraceResult()
        {
            Methods = new List<TraceResult>();
        }

    }
}