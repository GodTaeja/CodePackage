using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class MPoolManager
{
    private static MPoolManager _instance;

    /// <summary>
    ///     生命周期与游戏保持一致的池
    ///     储存着所有需要暂存的游戏对象
    /// </summary>
    private readonly GameObject _pool;

    private readonly Dictionary<string, GameObject> _prefabCache;

    private readonly Dictionary<GameObject, LeanGameObjectPool> Links;

    private MPoolManager()
    {
        _pool = new GameObject("MPool");
        Object.DontDestroyOnLoad(_pool);

        Links = new Dictionary<GameObject, LeanGameObjectPool>();
        _prefabCache = new Dictionary<string, GameObject>();
    }

    public static MPoolManager Instance
    {
        get { return _instance ?? (_instance = new MPoolManager()); }
    }

    public void Preload(string prefabName, int size = 5)
    {
        Preload(PrefabCacheFirst(prefabName), size);
    }

    public GameObject Spawn(string prefabName)
    {
        return Spawn(prefabName, Vector3.zero, Quaternion.identity, null);
    }

    public GameObject Spawn(string prefabName, Vector3 position)
    {
        return Spawn(prefabName, position, Quaternion.identity, null);
    }

    public GameObject Spawn(string prefabName, Vector3 position, Quaternion rotation)
    {
        return Spawn(prefabName, position, rotation, null);
    }

    public GameObject Spawn(string prefabName, Vector3 position, Quaternion rotation, Transform parent)
    {
        return Spawn(PrefabCacheFirst(prefabName), position, rotation, parent);
    }

    public void ReleaseGameObjectPool(string prefabName)
    {
        ReleaseGameObjectPool(Object.Instantiate(PrefabCacheFirst(prefabName)));
    }

    private GameObject PrefabCacheFirst(string prefabName)
    {
        GameObject prefab;
        //if (!_prefabCache.TryGetValue(prefabName, out prefab))
            
        //资源加载
        return null;
    }

    /// <summary>
    ///     池中预加载一定数量目标预制体的实例
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <param name="size">预加载数量</param>
    public void Preload(GameObject prefab, int size = 5)
    {
        if (prefab != null)
        {
            var gameObjectPool = default(LeanGameObjectPool);

            if (LeanGameObjectPool.TryFindPoolByPrefab(prefab, ref gameObjectPool) == false)
            {
                gameObjectPool = new GameObject(prefab.name + " Pool").AddComponent<LeanGameObjectPool>();
                gameObjectPool.transform.SetParent(_pool.transform);

                gameObjectPool.Prefab = prefab;
            }

            gameObjectPool.Preload = size;
            gameObjectPool.PreloadAll();
        }
        else
        {
            Debug.LogError("无法为 null 预制体做预加载");
        }
    }

    /// <summary>
    ///     从池中拿出一个目标预制体的实例
    /// </summary>
    /// <param name="prefab">目标预制体</param>
    /// <returns></returns>
    public GameObject Spawn(GameObject prefab)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        return Spawn(prefab, position, Quaternion.identity, null);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Spawn(prefab, position, rotation, null);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (prefab != null)
        {
            var gameObjectPool = default(LeanGameObjectPool);

            if (LeanGameObjectPool.TryFindPoolByPrefab(prefab, ref gameObjectPool) == false)
            {
                gameObjectPool = new GameObject(prefab.name + " Pool").AddComponent<LeanGameObjectPool>();
                gameObjectPool.transform.SetParent(_pool.transform);

                gameObjectPool.Prefab = prefab;
            }

            var clone = gameObjectPool.Spawn(position, rotation, parent);

            if (clone != null)
            {
                if (gameObjectPool.Recycle && gameObjectPool.Spawned >= gameObjectPool.Capacity)
                {
                    var existingPool = default(LeanGameObjectPool);

                    if (Links.TryGetValue(clone, out existingPool))
                    {
                        if (existingPool != gameObjectPool)
                            Links.Remove(clone);
                        else
                            return clone.gameObject;
                    }
                }

                Links.Add(clone, gameObjectPool);

                return clone.gameObject;
            }
        }
        else
        {
            Debug.LogError("Attempting to spawn a null prefab");
        }

        return null;
    }

    /// <summary>
    ///     回收所有实例至对应池
    /// </summary>
    public void DespawnAll()
    {
        for (var i = LeanGameObjectPool.Instances.Count - 1; i >= 0; i--) LeanGameObjectPool.Instances[i].DespawnAll();

        Links.Clear();
    }

    /// <summary>
    ///     回收目标实例至对应池
    /// </summary>
    /// <param name="clone">目标实例</param>
    /// <param name="delay">延迟时间</param>
    public void Despawn(GameObject clone, float delay = 0.0f)
    {
        if (clone != null)
        {
            var pool = default(LeanGameObjectPool);

            // Try and find the pool associated with this clone
            if (Links.TryGetValue(clone, out pool))
            {
                // Remove the association
                Links.Remove(clone);

                pool.Despawn(clone, delay);
            }
            else
            {
                if (LeanGameObjectPool.TryFindPoolByClone(clone, ref pool))
                {
                    pool.Despawn(clone, delay);
                }
                else
                {
                    Debug.LogWarning("You're attempting to despawn a gameObject that wasn't spawned from this pool",
                        clone);

                    // Fall back to normal destroying
                    Object.Destroy(clone);
                }
            }
        }
        else
        {
            Debug.LogWarning("You're attempting to despawn a null gameObject", clone);
        }
    }

    /// <summary>
    ///     清空目标实例池
    /// </summary>
    /// <param name="clone">目标实例</param>
    public void ReleaseGameObjectPool(GameObject clone)
    {
        foreach (var keyValuePair in Links)
            if (keyValuePair.Key == clone)
            {
                keyValuePair.Value.ReleaseAll();
                Links.Remove(keyValuePair.Key);
                return;
            }
    }

    /// <summary>
    ///     清空所有池
    /// </summary>
    public void ReleaseAllGameObjectPool()
    {
        for (var i = LeanGameObjectPool.Instances.Count - 1; i >= 0; i--) LeanGameObjectPool.Instances[i].ReleaseAll();

        Links.Clear();
    }
}