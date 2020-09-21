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
        private ConcurrentStack<TraceResult> timesQueue = new ConcurrentStack<TraceResult>();
        private ConcurrentStack<Stopwatch> stack = new ConcurrentStack<Stopwatch>();
        
        public void StartTrace()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            stack.Push(stopWatch);
        }
        public void StopTrace()
        {
            Stopwatch stopWatch = new Stopwatch();
            stack.TryPop(out stopWatch);
            stopWatch.Stop();
            
            int threadId = Thread.CurrentThread.ManagedThreadId;
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();

            TraceResult result = new TraceResult
            {
                Time = stopWatch.ElapsedMilliseconds,
                Methodlevel = stack.Count,
                MethodName = methodBase.Name,
                ClassName = methodBase.ReflectedType.ToString(),
                ThreadId = threadId
            };

            TraceResult temp = new TraceResult();
            timesQueue.TryPeek(out temp);
            if (temp != null)
            {
                if (temp.Methodlevel > 0)
                {
                    result.Methods.Add(temp);
                    timesQueue.TryPop(out _);
                }
            }


            timesQueue.Push(result);
        }
        public ProgramThreads TraceResult()
        {
            //TraceResult result = new TraceResult();
            //result.time = stopTime - startTime;
            //return result;
            TraceResult temp = new TraceResult();
            List<int> thredIds = new List<int>();
            ProgramThreads programThreads = new ProgramThreads();
            programThreads.Threads = new List<ThreadResult>();
            var methodsList = timesQueue.ToArray();
            foreach (var method in methodsList)
            {
                if (!thredIds.Contains(method.ThreadId))
                {
                    thredIds.Add(method.ThreadId);
                }
            }
            foreach (var id in thredIds)
            {
                programThreads.Threads.Add(new ThreadResult(id));
                foreach (var method in methodsList)
                {
                    if (method.ThreadId == id)
                    {
                        programThreads.Threads[^1].Time += method.Time;
                        programThreads.Threads[^1].Methods.Add(method);
                    }
                }
            }
            foreach (var thread in programThreads.Threads)
            {
                thread.Methods.Reverse();
            }
            programThreads.Threads.Reverse();
            return programThreads;
        }
    }
}
