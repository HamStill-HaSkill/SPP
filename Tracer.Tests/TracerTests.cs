using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using TracerLib;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

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
        static public class OneMethodRec
        {
            public static void Method(int i)
            {
                _tracer.StartTrace();
                if (i == 4)
                {
                    _tracer.StopTrace();
                }
                else
                {
                    Thread.Sleep(300);
                    Method(++i);
                    _tracer.StopTrace();
                }

            }
        }

        static public class ManyMethods
        {
            public static void Method1()
            {
                _tracer.StartTrace();
                Thread.Sleep(100);
                Method2();
                _tracer.StopTrace();
            }
            public static void Method2()
            {
                _tracer.StartTrace();
                Thread.Sleep(200);
                Method3();
                _tracer.StopTrace();
            }
            public static void Method3()
            {
                _tracer.StartTrace();
                Thread.Sleep(50);
                _tracer.StopTrace();
            }
            public static void Method4()
            {
                _tracer.StartTrace();
                Thread.Sleep(10);
                Method1();
                _tracer.StopTrace();
            }
        }
        static public class MethodInThread
        {
            public static void MainMethod()
            {
                Method1();
                var testThread = new Thread(() =>
                {
                    Method2();
                });
                testThread.Start();
                testThread.Join();
            }
            public static void Method1()
            {
                _tracer.StartTrace();
                Thread.Sleep(300);
                _tracer.StopTrace();
            }
            public static void Method2()
            {
                _tracer.StartTrace();
                Thread.Sleep(100);
                _tracer.StopTrace();
            }
        }
        static public class ManyThreads
        {
            public static void MainMethod()
            {
                Method1();
                var testThread1 = new Thread(() =>
                {
                    Method1();
                });
                var testThread2 = new Thread(() =>
                {
                    Method2();
                });
                var testThread3 = new Thread(() =>
                {
                    Method1();
                });
                testThread1.Start();
                testThread2.Start();
                testThread3.Start();
                testThread1.Join();
                testThread2.Join();
                testThread3.Join();
            }
            public static void Method1()
            {
                _tracer.StartTrace();
                Thread.Sleep(300);
                _tracer.StopTrace();
            }
            public static void Method2()
            {
                _tracer.StartTrace();
                Thread.Sleep(100);
                _tracer.StopTrace();
            }
        }
        public static int GetCountOfMethods(TraceResult[] methodsList, int count)
        {
            foreach (var method in methodsList)
            {
                if (method.Methods.Count > 0)
                {
                    count = GetCountOfMethods(method.Methods.ToArray(), ++count);
                }
            }
            return count;
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
        [TestMethod]
        public void ComparingMethodTime()
        {
            OneMethod.Method();
            long expected = 300;
            var actual = _tracer.TraceResult().Threads[0].Methods[0].Time;
            Assert.AreEqual(true, actual >= expected);
        }
        [TestMethod]
        public void ComparingMethodNamesInRecursion()
        {
            OneMethodRec.Method(4);
            string expected = " Method";
            string actual = "";
            for (int i = 0; i < _tracer.TraceResult().Threads[0].Methods.Count; i++)
            {
                actual += " " + (_tracer.TraceResult().Threads[0].Methods[i].MethodName);
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ComparingCountOfMethodsInRecursion()
        {
            OneMethodRec.Method(0);
            int expected = 4;
            var methodList = _tracer.TraceResult().Threads[0].Methods.ToArray();
            int actual = GetCountOfMethods(methodList, 0);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CallManyMethodsComparingMethodNames()
        {
            ManyMethods.Method1();
            ManyMethods.Method4();
            ManyMethods.Method3();
            string expected = " Method1 Method4 Method3";
            string actual = "";
            for (int i = 0; i < _tracer.TraceResult().Threads[0].Methods.Count; i++)
            {
                actual += " " + (_tracer.TraceResult().Threads[0].Methods[i].MethodName);
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CallManyMethodsComparingThreadTime()
        {
            ManyMethods.Method1();
            ManyMethods.Method4();
            ManyMethods.Method3();
            long expected = 410;
            var actual = _tracer.TraceResult().Threads[0].Time;
            Assert.AreEqual(true, actual >= expected);
        }
        [TestMethod]
        public void ComparingMethodNamesInThread()
        {
            MethodInThread.MainMethod();
            string expected = " Method1 Method2";
            string actual = "";
            for (int i = 0; i < _tracer.TraceResult().Threads.Count; i++)
            {
                actual += " " + (_tracer.TraceResult().Threads[i].Methods[0].MethodName);
            }
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestCountOfThread()
        {
            ManyThreads.MainMethod();
            long expected = 4;
            var actual = _tracer.TraceResult().Threads.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
