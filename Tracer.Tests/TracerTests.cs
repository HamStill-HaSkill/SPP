using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using TracerLib;
using System;
using Newtonsoft.Json;

namespace TracerLib
{
    [TestClass]
    public class TracerTests
    {
        static public ITracer _tracer;

        [TestInitialize]
        public void TestInitialize()
        {
            _tracer = new Tracer();
        }
        static public class OneMethod
        {
            public static void Method()
            {
                _tracer.StartTrace();
                Thread.Sleep(300);
                _tracer.StopTrace();
            }
        }
        [TestMethod]
        public void ComparingMethodNames()
        {
            OneMethod.Method();
            string expected = "Method";
            var actual = _tracer.TraceResult().Threads[0].Methods[0].MethodName;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ComparingClassNames()
        {
            OneMethod.Method();
            string expected = "OneMethod";
            var actual = _tracer.TraceResult().Threads[0].Methods[0].ClassName;
            Assert.AreEqual(expected, actual);
        }
    }
}
