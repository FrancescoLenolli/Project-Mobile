using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //Classe generica per i singleton
    private static T instance;

    public static T Instance
    {
        get
        {
            //Debug.Assert(instance != null);
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<T>();
                return instance;
            }
        }
    }

    public void Awake()
    {
        instance = this as T;
    }
}
