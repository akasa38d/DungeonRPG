using UnityEngine;
using System.Collections;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        checkInstance();
    }
    protected bool checkInstance()
    {
        if (this == Instance) { return true; }
        Destroy(this);
        return false;
    }

}
