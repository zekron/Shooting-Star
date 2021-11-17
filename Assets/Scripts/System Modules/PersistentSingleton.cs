using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    //public static T Instance { get; private set; }

    //protected virtual void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this as T;
    //    }
    //    else if (Instance != this)
    //    {
    //        Destroy(gameObject);
    //    }

    //    DontDestroyOnLoad(gameObject);
    //}

    public static T Instance
    {
        get
        {
            var objs = FindObjectsOfType<T>();
            if (objs.Length > 1)
            {
#if UNITY_EDITOR
                Debug.LogError("不应该存在多个单例！");
#endif
                for (int i = 1; i < objs.Length; i++)
                {
                    Destroy(objs[i].gameObject);
                }
                return instance;
            }
            else if (objs.Length == 1)
            {
                instance = objs[0];
            }

            if (instance == null)
            {
                var singleton = new GameObject();
                instance = singleton.AddComponent<T>();
                singleton.name = $"Singleton {typeof(T)}";
                DontDestroyOnLoad(singleton.gameObject);
            }
            else
            {
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    private static T instance;
}