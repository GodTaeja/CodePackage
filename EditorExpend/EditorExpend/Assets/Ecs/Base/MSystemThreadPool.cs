using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThreadPool = ThreadPooling.ThreadPool;
#if !RUNINSERVER
using UnityEngine;
#endif

public class MSystemThreadPool
{
    //TaskScheduler
    private static MSystemThreadPool _instance=null;
    static ThreadPool threadPool;
    static MSystemThreadPool()
    {
        _instance=new MSystemThreadPool();
    }

    public static MSystemThreadPool Instance
    {
        get
        {
            return _instance;
        }
    }


    private MSystemThreadPool()
    {

    }
    private int poolSize;
    //private List<WatchableThread> selfThreadControllers=new List<WatchableThread>();
    private List<WatchableThread> threads = new List<WatchableThread>();
    private Queue<Action> jobs = new Queue<Action>();

    public bool QueueUserWorkItem(Action callBack)
    {
        if (callBack == null)
            throw new NotSupportedException("A callback method cannot be null");
        lock (jobs)
        {
            jobs.Enqueue(callBack);
            Monitor.Pulse(jobs);
        }

        return true;
    }

    public bool QueueMultiUserWorkItem(List<Action> callBacks)
    {
        if (callBacks == null)
            throw new NotSupportedException("A callback method cannot be null");

        //List<Task> tasks=new List<Task>();
        foreach (var callBack in callBacks)
        {
            threadPool.EnqueueWorkItem(callBack);
            //Task task=new Task(callBack);
            //task.Start();
            //tasks.Add(task);
        }

        //Task.WaitAll(tasks.ToArray());
        return true;

        lock (jobs)
        {
            foreach (var callBack in callBacks)
            {
                jobs.Enqueue(callBack);
            }
            Monitor.PulseAll(jobs);
        }

        return true;
    }

    public bool QueueMultiMJob(List<MSystemJob> sjobs)
    {
        Debug.LogError(3333);
        if (sjobs == null)
            throw new NotSupportedException("A callback method cannot be null");

        //List<Task> tasks = new List<Task>();
        foreach (var callBack in sjobs)
        {
            threadPool.EnqueueWorkItem(callBack.Execute);
            //Task task = new Task(callBack.Execute);
            //task.Start();
            //tasks.Add(task);
        }

        //Task.WaitAll(tasks.ToArray());
        return true;
        lock (jobs)
        {
            foreach (var sjob in sjobs)
            {
                jobs.Enqueue(sjob.Execute);
            }
            Monitor.PulseAll(jobs);
        }

        return true;
    }

    public bool SetPoolSize(int size)
    {
        lock (threads)
        {
            poolSize = size;
            if (poolSize > threads.Count)
                spawnThreads();
            else if (poolSize < threads.Count)
            {
                lock (jobs) Monitor.PulseAll(jobs);
            }
        }

        return true;
    }

    private void spawnThreads()
    {
        while (threads.Count < poolSize)
        {
            WatchableThread t = new WatchableThread(this);
            threads.Add(t);
        }
    }

    private class WatchableThread
    {
        private MSystemThreadPool controller;
        public Thread Thread;
        public bool IsExecuting = false;

        public WatchableThread(MSystemThreadPool controller)
        {
            this.controller = controller;
            Thread =new Thread(ConsumeJobs);
            Thread.Name = "wt";
            Thread.Start();
            Debug.LogError(222);
        }

        public void Stop()
        {

        }

        private void ConsumeJobs()
        {
            Action job;
            Debug.LogError(111);
            while (true)
            {
                IsExecuting = true;
                if (controller.killThreadIfNeeded()) return;

                lock (controller.jobs)
                {
                    while (controller.jobs.Count == 0 && !(controller.poolSize < controller.threads.Count))
                    {
                        IsExecuting = false;
                        Monitor.Wait(controller.jobs);
                    }
                    if (controller.killThreadIfNeeded()) return;
                    IsExecuting = true;
                    job = controller.jobs.Dequeue();
                }

                try
                {
                    //TaskScheduler
                    job?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }


            }
        }
    }
    

    private bool killThreadIfNeeded()
    {
        if (poolSize < threads.Count)
        {
            lock (threads)
            {
                if (poolSize < threads.Count)
                {
                    WatchableThread wt = null;
                    foreach (var tc in threads)
                    {
                        if (tc.Thread == Thread.CurrentThread)
                        {
                            wt = tc;
                        }
                    }
                    if(wt != null)
                        threads.Remove(wt);
                    return true;
                }
            }
        }

        return false;
    }
    public int PoolSize { get { return poolSize; } }
    public int ActualPoolSize { get { return threads.Count; } }

    public bool IsExecuting()
    {
        //DedicatedThreadPool.WaitForThreadsExit();
        //threadpool里面有wait了
        threadPool.WaitForEveryWorkerIdle();
        return false;
        foreach (var wt in threads)
        {
            if (wt.IsExecuting)
                return true;
        }

        return false;
    }

    public void Start(int size)
    {
        //SetPoolSize(size);
        if (threadPool == null || threadPool.NumberOfThreads != size)
        {
            threadPool?.ShutDown();
            threadPool = new ThreadPool(size, "battle");
        }
        threadPool.Wakeup();
    }

    public void Stop()
    {
        //SetPoolSize(0);
        threadPool?.Sleep();
    }
}


