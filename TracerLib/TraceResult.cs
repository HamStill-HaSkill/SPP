using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TracerLib
{
    public class TraceResult
    {
        public long Time { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public int ThreadId { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public int Methodlevel { get; set; }
        public List<TraceResult> Methods {get; internal set; }
        public TraceResult()
        {
            Methods = new List<TraceResult>();
        }

    }
}