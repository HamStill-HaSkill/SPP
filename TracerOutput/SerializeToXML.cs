using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TracerLib;

namespace TracerOutput
{
    public class SerializeToXML : ISerialize
    {
        public string Serialize(ITracer tracer)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProgramThreads));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, tracer.TraceResult());
                return textWriter.ToString();
            }
        }
    }
}
