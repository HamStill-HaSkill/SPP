using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TracerLib;


namespace TracerOutput
{
    class OutputResultToFile : IOutputResult
    {
        public string SavePath { get; set; } 
        public void OutputResult(string result)
        {
            using (StreamWriter sw = new StreamWriter(SavePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(result);
            }
        }
        public OutputResultToFile()
        {
            SavePath = "test.json";
        }

    }
}
