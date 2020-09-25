using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace TracerLib
{
    public class Tracer : ITracer
    {
        private ConcurrentDictionary<int, MethodsWatch> threadInfo = new ConcurrentDictionary<int, MethodsWatch>();

        public void StartTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (threadInfo.ContainsKey(threadId))
            {
                threadInfo[threadId].watches.Push(stopWatch);
            }
            else
            {
                threadInfo[threadId] = new MethodsWatch();
                threadInfo[threadId].watches.Push(stopWatch);
            }
        }
        public void StopTrace()
        {
            var stopWatch = new Stopwatch();
            int threadId = Thread.CurrentThread.ManagedThreadId;


            if(threadInfo[threadId].watches.Count > 0)
                while (!threadInfo[threadId].watches.TryPop(out stopWatch));
            stopWatch.Stop();

            // Get info of the current method
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();

            TraceResult result = new TraceResult
            {
                Time = stopWatch.ElapsedMilliseconds,
                Methodlevel = threadInfo[threadId].watches.Count,
                MethodName = methodBase.Name,
                ClassName = methodBase.DeclaringType.Name,
                ThreadId = threadId
            };

            TraceResult temp = new TraceResult();
            if (threadInfo[threadId].methods.Count > 0)
                while (!threadInfo[threadId].methods.TryPeek(out temp)) ;

            if (temp.MethodName != null)
            {
                if (temp.Methodlevel > 0)
                {
                    result.Methods.Add(temp);
                    if (threadInfo[threadId].methods.Count > 0)
                        while (!threadInfo[threadId].methods.TryPop(out temp)) ;
                }
            }
            threadInfo[threadId].methods.Push(result);
        }
        public ProgramThreads TraceResult()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            TraceResult temp = new TraceResult();         
            ProgramThreads programThreads = new ProgramThreads();
            programThreads.Threads = new List<ThreadResult>();
            
            // Add methods to their parent threads
            foreach (var item in threadInfo)
            {
                programThreads.Threads.Add(new ThreadResult());
                programThreads.Threads[^1].Id = item.Key;
                programThreads = GetMethodListGropedById(programThreads, item.Value.methods.ToArray(), item.Key);
            }
            foreach (var thread in programThreads.Threads)
            {
                thread.Methods.Reverse();
            }

            return programThreads;
        }
     
        public static ProgramThreads GetMethodListGropedById(ProgramThreads programThreads, TraceResult[] methodsList, int id)
        {
            foreach (var method in methodsList)
            {
                    programThreads.Threads[^1].Time += method.Time;
                    programThreads.Threads[^1].Methods.Add(method);
            }
            return programThreads;
        }
    }
}
