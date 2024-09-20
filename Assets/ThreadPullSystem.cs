using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadPullSystem
{
    public static List<ThreadPullet> threads = new();
    public static int max = 10;
    public static ThreadPullet SearchMinTask()
    {
        int min = 100000;
        ThreadPullet tp=null;
        for (int i = 0; i < max; i++)
        {
            if (threads[i].TaskCount < min)
            {
                min = threads[i].TaskCount;
                tp = threads[i];
            }
        }
        return tp;
    }
    public static ThreadPullet Add(ParameterizedThreadStart pts)
    {
        ThreadPullet tp = null;
        if (threads.Count < max)
        {
            
            Thread thread = new(pts);
            tp = new ThreadPullet(thread);
            tp.thread.Start(pts.Method);
            threads.Add(tp);
        }
        else
        {
            tp = SearchMinTask();
            tp.thread.Start(pts.Method);
            tp.TaskCount++;
        }
        return tp;

    }
    public static void Remove(ThreadPullet tp,Action method)
    {
        
        if (threads.Count < max)
        {
           
            threads.Remove(tp);
            tp.thread.Abort(method);
        }
        else
        {
            tp.TaskCount--;
            tp.thread.Abort(method);
        }
       

    }
}
public class ThreadPullet
{
    public Thread thread;
    public int TaskCount;

    public ThreadPullet(Thread thread)
    {
        this.thread = thread;
        TaskCount = 1;
    }
}
