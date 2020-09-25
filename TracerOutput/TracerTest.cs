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
            TracerTestSecond testik = new TracerTestSecond(_tracer);
            var testThread = new Thread(() =>
            {
                testik.TesterBBBBBBBBBB();
            });
            testThread.Start();
            testThread.Join();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }
        public void TestTestTset()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            TracerTestSecond testik = new TracerTestSecond(_tracer);
            testik.TesterAAAAAAAAA();
            _tracer.StopTrace();


        }
    }
    class TracerTestSecond
    {
        private ITracer _tracer;
        internal TracerTestSecond(ITracer tracer)
        {
            _tracer = tracer;
        }
        public void TesterAAAAAAAAA()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
        public void TesterBBBBBBBBBB()
        {
            _tracer.StartTrace();
            TracerTestSecond testik = new TracerTestSecond(_tracer);
            testik.TesterAAAAAAAAA();
            Thread.Sleep(500);
            _tracer.StopTrace();
        }
    }
}
