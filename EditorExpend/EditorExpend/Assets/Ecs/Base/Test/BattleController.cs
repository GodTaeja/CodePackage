

using System;
using System.Linq;
using System.Text;
using System.Threading;
using TrueSync;
using UnityEngine;

public class BattleController
{
    private readonly MEntityManager _entityManager;

    private CombatStatus _combatStatus;

    //逻辑帧间隔, 单位：秒
    private float _logicFrameDeltaTime = 0.1f;
    private float _lastLogicTime;
    private float _realTime;
    private float _lastKeyFrameTime;
    private bool _hasFirstLogic = false;
    
    /// <summary>
    /// 逻辑层系统
    /// </summary>

    //移动系统
    private MovementSystem _movementSystem;

    /// <summary>
    /// 表现层系统
    /// </summary>

    private PositionSyncSystem _positionSyncSystem;


    private System.Threading.Thread logicThread = null; 
    enum CombatStatus
    {
        None,
        WaitForViewSpawn,
        Fight,
        Over
    }

    public BattleController(Transform sceneRoot, Camera mainCamera)
    {
        _entityManager = new MEntityManager();
        InitSystems();

    }

    public MEntityManager GetEntityManager()
    {
        return _entityManager;
    }

    private void InitSystems()
    {
        _movementSystem = new MovementSystem(_entityManager);

        //表现层系统

        _positionSyncSystem = new PositionSyncSystem(_entityManager);

    }

    bool logicFrameOver = false;
    object logicFrameLock=new object();
    // Update is called once per frame
    public void Update(float deltaTime)
    {
        if (_combatStatus == CombatStatus.None)
            return;
        
        if (_combatStatus == CombatStatus.Over)
        {
            logicFrameOver = true;
        }
        //插值, view帧总是从上一个逻辑帧开始，逐步逼近当前的逻辑帧
        var inter = (_realTime - _lastKeyFrameTime) / _logicFrameDeltaTime;
        if (logicFrameOver)
            inter = 0;
#if OUTPUTSYSTEMTIME
        //if (_combatStatus == CombatStatus.Fight || _combatStatus == CombatStatus.Over)
            Debug.Log("inter:" + inter+ " _realTime:"+ _realTime+ " _lastLogicTime:"+ _lastKeyFrameTime + " "+ _combatStatus+" "+ logicFrameOver);
#endif
        //如果logic执行太长，超出的时间就不更新view。或者，logic执行完毕之后继续更新view
        if ((logicFrameOver /*&& !Monitor.IsEntered(logicFrameLock)*/))
        {
            
            //Monitor.Enter(logicFrameLock);
            try
            {
                //inter = (_realTime - _lastLogicTime) / _logicFrameDeltaTime;
                var result = UpdateView(0, deltaTime, true);
                if (_combatStatus == CombatStatus.Fight && result)
                {
                    Debug.LogError("CombatStatus.Over");
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            logicFrameOver = false;
            _lastKeyFrameTime = _realTime;
            //Monitor.Exit(logicFrameLock);
        }
        else
        //if ((_realTime - _lastKeyFrameTime <= _logicFrameDeltaTime || _combatStatus == CombatStatus.Over)/*&& Mathf.Abs(_lastLogicTime- _lastSyncTime) <0.01f*/)
        {
            //同步逻辑update执行完之后的第一帧
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            
            {
                try
                {
                    UpdateView(inter, deltaTime, false);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                
            }
            //if (_combatStatus == CombatStatus.Fight || _combatStatus == CombatStatus.Over)
            //    _realTime += deltaTime;


        }
        if (_combatStatus == CombatStatus.Fight || _combatStatus == CombatStatus.Over)
            _realTime += deltaTime;


    }

    /** 
     * logic 线程和主线程同步logic完成后的第一帧 
     */
    private void LogicThreadUpdater()
    {
        while (_combatStatus != CombatStatus.Over&&_combatStatus!=CombatStatus.None)
        {
            //Debug.LogError("LogicThreadUpdater 111:"+ logicFrameOver);
            Thread.MemoryBarrier();
            if (logicFrameOver)
            {
                Thread.Sleep(1);
                continue;
            }

            //Debug.LogError("LogicThreadUpdater 222:"+ _realTime);
            float localRealTime = _realTime;
            //lock (logicFrameLock)
            //Monitor.Enter(logicFrameLock);
            {
                //Debug.LogError("LogicThreadUpdater 333");
                //Debug.LogWarning("logicFrameLock:" + stopwatch.Elapsed.TotalMilliseconds);
                if (localRealTime - _lastLogicTime >= _logicFrameDeltaTime)
                {
#if OUTPUTSYSTEMTIME
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
#endif
                    int count = 1;//(int)((realTime - _lastLogicTime) / _logicFrameDeltaTime);
                    //if (!HasFirstLogic)
                    //{
                    //    count = 1;
                    //    _realTime = 0;
                    //    HasFirstLogic = true;
                    //}
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            UpdateLogic(_logicFrameDeltaTime);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                        
                        battleTick++;
                    }

                    logicFrameOver = true;
                    _lastLogicTime = _lastLogicTime+ _logicFrameDeltaTime*count;
                    if (!_hasFirstLogic)
                    {
                        _realTime = _lastLogicTime;
                        _hasFirstLogic = true;
                    }
#if OUTPUTSYSTEMTIME
                    //if (stopwatch.Elapsed.TotalMilliseconds>100)
                    Debug.LogError("UpdateLogic:" + stopwatch.Elapsed.TotalMilliseconds + " " + battleTick + " "+ _lastLogicTime+" "+ localRealTime);
#endif
                }
            }
            //Monitor.Exit(logicFrameLock);

        }

        Debug.Log("LogicThreadOver:" + _combatStatus);

    }
    //ecs
    void UpdateLogic(float deltaTime)
    {
        //按时间间隔改变数据的系统需要放在创建entity的系统的前面，因为entity被创建出来的
        // 这一帧并没有“经过一段时间”，不需要改变数据
        //_pathFindingSystem.Update(deltaTime);

        _movementSystem.Update(deltaTime);
    }

    //返回view是否完成
    bool UpdateView(float _inter, float deltaTime, bool _logicFrameOver)
    {
        float inter = Mathf.Clamp01(_inter);

        _positionSyncSystem.Update(inter, _logicFrameOver);

        return false;
    }


    public void Clear()
    {
        DestroyAllEntities();
        //刷新一次view
        UpdateView(0, 0, false);
        _combatStatus = CombatStatus.None;
    }

    void DestroyAllEntities()
    {
        _entityManager.Clear();
    }


    private int battleTick = 0;

    public void BattleExit()
    {
        //if (Monitor.IsEntered(logicFrameLock))
        //    Monitor.Exit(logicFrameLock);
        if(logicThread!=null&&logicThread.IsAlive)
            logicThread.Abort();
        logicThread = null;
        DestroyAllEntities();
        MSystemThreadPool.Instance.Stop();
        //        _actorSpawnSystem.Clear();
        //        _effectSystem.Clear();
    }

}
