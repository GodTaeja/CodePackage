using System;
using System.Collections.Generic;
using System.Threading;
#if !RUNINSERVER
using UnityEngine;
#endif

public class MSystem
{
    public MEntityManager EntityManager;

    //for ilruntime adaptor
    public MSystem()
    {
    }
    public virtual bool IsLogicComponents => true;
    public virtual bool EnableJob => false;
    public virtual int[] HandleComponents
    {
        get { return null; }
    }

    public virtual int[] NotContainsComponents
    {
        get { return null; }
    }

    public virtual MSystemJob GetJob(MEntity entity)
    {
        return null;
    }

    public virtual void Update(float deltaTime)
    {
        //if (EnableJob)
        UpdateJobs(deltaTime);
    }

    protected virtual void UpdateJobs(float deltaTime)
    {
        //EntityManager.GetComponents<>()

        List<MEntity> matches = SearchEntities();
        if (matches == null || matches.Count <= 0)
            return;
        List<MSystemJob> jobs = new List<MSystemJob>();
        foreach (MEntity entity in matches)
        {
            MSystemJob job = GetJob(entity);
            job.deltaTime = deltaTime;
#if !RUNINSERVER
            if (EnableJob)
            {
                jobs.Add(job);
                //MSystemThreadPool.Instance.QueueUserWorkItem(job.Execute);// 放到地下批量操作
            }
            else
                job.Execute();
#else
            job.Execute();
#endif
        }
#if !RUNINSERVER
        if (EnableJob)
        {
            MSystemThreadPool.Instance.QueueMultiMJob(jobs);
            WaitForJobDone();
        }
#endif
    }

    protected virtual void WaitForJobDone()
    {
        //TODO wait for jobs done
#if !RUNINSERVER
        while (MSystemThreadPool.Instance.IsExecuting())
        {
            //Monitor.Wait();
            //WaitHandle.WaitAll()
        }
#endif
    }

    protected List<MEntity> SearchEntities()
    {
        if (HandleComponents == null || HandleComponents.Length <= 0)
            return null;
        List<MComponent> cs = null;
        if (IsLogicComponents)
            cs = EntityManager.GetLogicComponents<MComponent>(HandleComponents[0]);
        else
            cs = EntityManager.GetComponents<MComponent>(HandleComponents[0]);
        int count = cs.Count;
        List<MEntity> matches = new List<MEntity>();
        for (int i = 0; i < count; i++)
        {
            MEntity entity = cs[i].Entity;
            //check not contain first
            bool match = true;
            if (NotContainsComponents != null)
            {
                foreach (int type in NotContainsComponents)
                {
                    if (entity.HasComponent(type))
                    {
                        match = false;
                        break;
                    }

                }
                if (!match)
                    continue;
            }
            //check has components
            foreach (int type in HandleComponents)
            {
                if (!entity.HasComponent(type))
                {
                    match = false;
                    break;
                }

            }
            if (!match)
                continue;
            matches.Add(entity);
        }

        return matches;
    }

}


