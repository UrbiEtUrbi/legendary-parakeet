using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolManager : GenericSingleton<PoolManager>
{
    private static Dictionary<string,Pool> _poolDictionary;

    [SerializeField, ReorderableListExposed]
    List<PoolDefinition> pooledObjects;

    
    protected override void Awake()
    {
      
        base.Awake();
        Instance.Init();
    }

    public void Init()
    {
        _poolDictionary = new Dictionary<string, Pool>();
        foreach (var poolDeifinition in pooledObjects)
        {
            CreatePool(poolDeifinition);
        }
    }

    void CreatePool(PoolDefinition poolDefinition)
    {
        if (!_poolDictionary.ContainsKey(poolDefinition.poolObject.gameObject.name))
        {
            _poolDictionary.Add(poolDefinition.poolObject.gameObject.name, new Pool(poolDefinition));

            for (int i = 0; i < poolDefinition.initialPoolSize; i++)
            {
                CreateObjectInstance(poolDefinition);
            }
        }
    }

    void CreateObjectInstance(PoolDefinition poolDefinition)
    {
        ObjectInstance objectInPool = new ObjectInstance(Instantiate(poolDefinition.poolObject.gameObject), poolDefinition.poolObject.gameObject.name);
        objectInPool.SetParent(transform);
        _poolDictionary[poolDefinition.poolObject.gameObject.name].queue.Enqueue(objectInPool);
    }


    public static T Spawn<T>(Transform parent, Vector3 position = default, Quaternion rotation = default) where T : PoolObject
    {
        return Spawn<T>(typeof(T).Name, parent, position, rotation);
    }

    public static T Spawn<T>(string key, Transform parent, Vector3 position = default, Quaternion rotation = default) where T : PoolObject
    {
        if (!_poolDictionary.ContainsKey(key))
        {
            Debug.LogError(key + " Pool not found!!");
            return default;
        }
        else
        {
            if (_poolDictionary[key].queue.Count == 0)
            {
                Instance.CreateObjectInstance(_poolDictionary[key].poolDefinition);
            }
            ObjectInstance objectInPool = _poolDictionary[key].queue.Dequeue();
            objectInPool.Reuse(parent, position, rotation);
            _poolDictionary[key].spawned.Add(objectInPool);
            return (T)objectInPool.PoolObject;
        }
    }

    public static void Despawn(PoolObject poolObject)
    {
        if (!_poolDictionary.ContainsKey(poolObject.key))
        {
            Debug.LogError($"Can't despawn {poolObject.key} Pool not found!!");
            return;
        }
        else
        {
            var pool = _poolDictionary[poolObject.key];
            var idx = pool.spawned.FindIndex(x => x.PoolObject == poolObject);
            if (idx == -1)
            {
                Debug.LogError($"Can't despawn {poolObject.key} object not from pool");
                Destroy(poolObject.gameObject);
                return;
            }
            else
            {
                var objectInstance = pool.spawned[idx];
                if (pool.queue.Count >= pool.poolDefinition.maxPoolSize)
                {
                    objectInstance.Destroy();
                    Destroy(objectInstance.PoolObject.gameObject);
                    objectInstance = null;
                }
                else
                {
                    if (objectInstance == null || objectInstance.PoolObject == null || objectInstance.PoolObject.gameObject == null)
                    {
                        return;
                    }
                    objectInstance.PoolObject.gameObject.SetActive(false);
                    if (Instance != null)
                    {
                        objectInstance.SetParent(Instance.transform);
                    }
                    pool.queue.Enqueue(objectInstance);
                }
            }
        }
    }

   
}

[System.Serializable]
public class PoolDefinition
{
    public PoolObject poolObject;
    public int initialPoolSize;
    public int maxPoolSize;
}

public class Pool {
    public Queue<ObjectInstance> queue;
    public List<ObjectInstance> spawned;
    public PoolDefinition poolDefinition;

    public Pool(PoolDefinition poolDefinition)
    {
        this.poolDefinition = poolDefinition;
        queue = new Queue<ObjectInstance>();
        spawned = new List<ObjectInstance>();
    }
}
