using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private ObjectPool[] enemyPools;
    [SerializeField] private ObjectPool[] playerProjectilePools;
    [SerializeField] private ObjectPool[] enemyProjectilePools;
    [SerializeField] private ObjectPool[] vFXPools;
    [SerializeField] private ObjectPool[] itemPools;
    [SerializeField] private ObjectPool[] playerModelPools;

    private static Dictionary<GameObject, ObjectPool> objectPoolDictionary;

    private void Awake()
    {
        objectPoolDictionary = new Dictionary<GameObject, ObjectPool>();

        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
        Initialize(itemPools);
        Initialize(playerModelPools);
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
        CheckPoolSize(itemPools);
    }
#endif

    private void CheckPoolSize(ObjectPool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}!",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    private void Initialize(ObjectPool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (objectPoolDictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools! Prefab: " + pool.Prefab.name);

                continue;
            }
#endif
            objectPoolDictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// <para>Return a specified <paramref name="prefab"></paramref> gameObject in the pool.</para>
    /// <para>根据传入的 <paramref name="prefab"></paramref> 参数，返回对象池中预备好的游戏对象。</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <returns>
    /// <para>Prepared gameObject in the pool.</para>
    /// <para>对象池中预备好的游戏对象。</para>
    /// </returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!objectPoolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return objectPoolDictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// <para>Release a specified <paramref name="prefab"></paramref> gameObject in the pool at specified <paramref name="position"></paramref>.</para>
    /// <para>根据传入的 <paramref name="prefab"></paramref> 参数，在 <paramref name="position"></paramref> 参数位置释放对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!objectPoolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return objectPoolDictionary[prefab].PreparedObject(position);
    }

    public static GameObject[] Release(GameObject prefab, Vector3[] position)
    {
#if UNITY_EDITOR
        if (!objectPoolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return objectPoolDictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// <para>Release a specified <paramref name="prefab"></paramref> gameObject in the pool at specified <paramref name="position"></paramref> and <paramref name="rotation"></paramref>.</para>
    /// <para>根据传入的 <paramref name="prefab"></paramref> 参数和 <paramref name="rotation"></paramref> 参数，在 <paramref name="position"></paramref> 参数位置释放对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>指定的旋转值。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!objectPoolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return objectPoolDictionary[prefab].PreparedObject(position, rotation);
    }

    /// <summary>
    /// <para>Release a specified <paramref name="prefab"></paramref> gameObject in the pool at specified <paramref name="position"></paramref>, <paramref name="rotation"></paramref> and <paramref name="localScale"></paramref>.</para>
    /// <para>根据传入的 <paramref name="prefab"></paramref> 参数, <paramref name="rotation"></paramref> 参数和 <paramref name="localScale"></paramref> 参数，在 <paramref name="position"></paramref> 参数位置释放对象池中预备好的游戏对象。</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>指定的游戏对象预制体。</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>指定释放位置。</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>指定的旋转值。</para>
    /// </param>
    /// <param name="localScale">
    /// <para>Specified scale.</para>
    /// <para>指定的缩放值。</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!objectPoolDictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return objectPoolDictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}