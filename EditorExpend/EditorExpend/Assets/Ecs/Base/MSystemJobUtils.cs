using System;
using System.Collections.Generic;
using System.Linq;

public class MSystemJobUtils
{
    public static void Run()
    {

    }
}

public abstract class MSystemJob
{
    public float deltaTime;
    public abstract void Execute();
}

public class MSystemJob<T> : MSystemJob
{
    private T t;
    public Action<T,float> Action;

    public MSystemJob(T t,Action<T, float> action)
    {
        this.t = t;
        this.Action = action;
    }
    public override void Execute()
    {
        Action(t, deltaTime);
    }
}

public class MSystemJob<T0,T1> : MSystemJob
{
    private T0 t0;
    private T1 t1;
    public Action<T0,T1, float> Action;

    public MSystemJob(T0 t0,T1 t1, Action<T0,T1, float> action)
    {
        this.t0 = t0;
        this.t1 = t1;
        this.Action = action;
    }
    public override void Execute()
    {
        Action(t0,t1, deltaTime);
    }
}

public class MSystemJob<T0, T1,T2> : MSystemJob
{
    private T0 t0;
    private T1 t1;
    private T2 t2;
    public Action<T0, T1,T2, float> Action;

    public MSystemJob(T0 t0, T1 t1,T2 t2, Action<T0, T1,T2, float> action)
    {
        this.t0 = t0;
        this.t1 = t1;
        this.t2 = t2;
        this.Action = action;
    }
    public override void Execute()
    {
        Action(t0, t1,t2, deltaTime);
    }
}

public class MSystemJob<T0, T1, T2,T3> : MSystemJob
{
    private T0 t0;
    private T1 t1;
    private T2 t2;
    private T3 t3;
    public Action<T0, T1, T2,T3, float> Action;

    public MSystemJob(T0 t0, T1 t1, T2 t2,T3 t3, Action<T0, T1, T2,T3, float> action)
    {
        this.t0 = t0;
        this.t1 = t1;
        this.t2 = t2;
        this.t3 = t3;
        this.Action = action;
    }
    public override void Execute()
    {
        Action(t0, t1, t2,t3,deltaTime);
    }
}

public class MSystemJob<T0, T1, T2, T3,T4> : MSystemJob
{
    private T0 t0;
    private T1 t1;
    private T2 t2;
    private T3 t3;
    private T4 t4;
    public Action<T0, T1, T2, T3,T4, float> Action;

    public MSystemJob(T0 t0, T1 t1, T2 t2, T3 t3,T4 t4, Action<T0, T1, T2, T3,T4, float> action)
    {
        this.t0 = t0;
        this.t1 = t1;
        this.t2 = t2;
        this.t3 = t3;
        this.t4 = t4;
        this.Action = action;
    }
    public override void Execute()
    {
        Action(t0, t1, t2, t3,t4, deltaTime);
    }
}