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
        private ConcurrentStack<TraceResult> timesStack = new ConcurrentStack<TraceResult>();
        private ConcurrentStack<Stopwatch> watchesStack = new ConcurrentStack<Stopwatch>();
        
        public void StartTrace()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            watchesStack.Push(stopWatch);
        }
        public void StopTrace()
        {
            var stopWatch = new Stopwatch();
            if (watchesStack.Count > 0)
                while (!watchesStack.TryPop(out stopWatch));
            stopWatch.Stop();

            // Get id of the current thread
            int threadId = Thread.CurrentThread.ManagedThreadId;
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();

            TraceResult result = new TraceResult
            {
                Time = stopWatch.ElapsedMilliseconds,
                Methodlevel = watchesStack.Count,
                MethodName = methodBase.Name,
                ClassName = methodBase.DeclaringType.Name,
                ThreadId = threadId
            };

            // Add child methods to their parent methods
            TraceResult temp = new TraceResult();
            if (timesStack.Count > 0)
                while(!timesStack.TryPeek(out temp));
            if (temp != null)
            {
                if (temp.Methodlevel > 0)
                {
                    result.Methods.Add(temp);
                    if (timesStack.Count > 0)
                        while (!timesStack.TryPop(out temp)) ;
                }
            }
            timesStack.Push(result);
        }
        public ProgramThreads TraceResult()
        {
            // Get all thread id
            TraceResult temp = new TraceResult();         
            var methodsList = timesStack.ToArray();
            List<int> thredIds = GetThreadIds(methodsList, new List<int>());

            ProgramThreads programThreads = new ProgramThreads();
            programThreads.Threads = new List<ThreadResult>();
            
            // Add methods to their parent threads
            foreach (var id in thredIds)
            {
                programThreads.Threads.Add(new ThreadResult());
                programThreads.Threads[^1].Id = id;
                programThreads = GetMethodListGropedById(programThreads, methodsList, id);
            }
            foreach (var thread in programThreads.Threads)
            {
                thread.Methods.Reverse();
            }

            programThreads.Threads.Reverse();
            return programThreads;
        }
        
        public static List<int> GetThreadIds(TraceResult[] methodsList, List<int> thredIds)
        {
            foreach (var method in methodsList)
            {
                if (method.Methods.Count > 0)
                {
                    thredIds = GetThreadIds(method.Methods.ToArray(), thredIds);
                }
                if (!thredIds.Contains(method.ThreadId))
                {
                    thredIds.Add(method.ThreadId);
                }
            }
            return thredIds;
        }

        public static ProgramThreads GetMethodListGropedById(ProgramThreads programThreads, TraceResult[] methodsList, int id)
        {
            foreach (var method in methodsList)
            {
                if (method.ThreadId == id)
                {
                    programThreads.Threads[^1].Time += method.Time;
                    programThreads.Threads[^1].Methods.Add(method);
                }
            }
            return programThreads;
        }
    }
}
