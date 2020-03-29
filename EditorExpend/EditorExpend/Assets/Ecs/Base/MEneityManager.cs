using System;
using System.Collections.Generic;
#if !RUNINSERVER

#endif

public class MEntityManager
{
    private readonly Dictionary<int, MEntity> _entities;
    private readonly ComponentGroup _componentGroup;
    private int _nextEntityId = 0;
#if !RUNINSERVER
    private Pool<MEntity> _entityPool;
#endif
    public MComponent[] _singletonComponents;

    public MEntityManager(int capcity = 1024)
    {
        _singletonComponents = new MComponent[64];
        _entities = new Dictionary<int, MEntity>(capcity);
        _componentGroup = new ComponentGroup();
#if !RUNINSERVER
        MEntity Create()
        {
            return new MEntity(_nextEntityId++, this);
        }

        _entityPool = PoolFactory.Create(Create, null, null, null, 512);
#endif
    }

    public T AddSingletonComponent<T>(int componentType, T component) where T : MComponent
    {
        _singletonComponents[componentType] = component;
        return component;
    }


    public List<T> GetLogicComponents<T>(int componentType) where T : MComponent
    {
        bool IsLogicAlive(MEntity entity)
        {
            return entity != null && entity.IsLogicALive;
        }

        return GetComponents<T>(componentType, IsLogicAlive);
    }

    //private readonly object[] _listPool = new object[64];//可能造成多线程读写问题


    public List<T> GetComponents<T>(int componentType, Func<MEntity, bool> condition = null) where T : MComponent
    {

#if DEBUG
        MEntity.CheckComponentParams<T>(componentType);
#endif

        int index = componentType;
        //UnityEngine.Debug.Log("_listPool[index]:"+ _listPool[index]);
        List<T> ret = new List<T>();//List<T>);_listPool[index];
        //if (ret == null)
        //{
        //    ret = new List<T>(128);
        //    _listPool[index] = ret;
        //}
        //ret.Clear();

        var list = _componentGroup.GetComponents(componentType);
        int n = list.Count;
        int n2 = n;

        for (int i = n - 1; i >= 0; i--)
        {
            var c = list[i];
            //延迟删除
            if (c.Entity == null || !_entities.ContainsKey(c.Entity.Id))
            {
                //和最后一个交换, 然后移除最后一个
                n2--;
                list[i] = list[n2];
                list.RemoveAt(n2);
            }
            else
            {
                if (condition == null || condition.Invoke(c.Entity))
                {
                    ret.Add(c as T);
                }
            }
        }

        return ret;
    }

    public MEntity CreateEntity()
    {
        //        var e = new MEntity(_nextEntityId++, _componentGroup);
#if !RUNINSERVER
        var e = _entityPool.Get();
#else
        var e = new MEntity(_nextEntityId++,this);
#endif
        e.OnComponentAdded = OnComponentAdded;
        e.IsLogicALive = true;
        _entities.Add(e.Id, e);
        return e;
    }

    private void OnComponentAdded(MEntity entity, int componentType, MComponent component)
    {
        _componentGroup.AddComponent(component, componentType);
    }


    public void Remove(MEntity entity, bool clearEntity = true)
    {

        if (clearEntity)
            OnEntityRemove(entity);
        else
        {
            //UnityEngine.Debug.LogWarning("remove entity:" + entity.Id + " " + clearEntity);
            entity.DestroyAllComponets();
        }
        _entities.Remove(entity.Id);
    }

    void OnEntityRemove(MEntity entity)
    {
        entity.OnComponentAdded = null;
        entity.Name = null;
        entity.IsLogicALive = false;
        entity.RemoveAllComponents();
#if !RUNINSERVER
        _entityPool.Put(entity);
#endif
    }

    /*
    public bool Contains(MEntity entity)
    {
        return _entities.ContainsKey(entity.Id);
    }
    */

    public void Clear()
    {
        foreach (var entity in _entities.Values)
        {
            OnEntityRemove(entity);
        }

        _entities.Clear();
        _componentGroup.Clear();
        Array.Clear(_singletonComponents, 0, _singletonComponents.Length);
    }

    public bool HasSingletonComponent(int componentType)
    {
        return _singletonComponents[componentType] != null;
    }

    public T GetSingletonComponent<T>(int componentType) where T : MComponent
    {
        int index = componentType;
        var ret = _singletonComponents[index];
        if (ret == null)
        {
            throw new Exception($"Can't find Singleton Component {componentType}");
        }

        var ret2 = ret as T;
        if (ret != null && ret2 == null)
        {
            throw new Exception($"Incorrect compnent type: require - {typeof(T).Name}, but real - {componentType}");
        }

        return ret2;
    }

    public T GetSingletonComponentCanBeNull<T>(int componentType) where T : MComponent
    {
        int index = componentType;
        var ret = _singletonComponents[index];

        var ret2 = ret as T;
        return ret2;
    }
}



