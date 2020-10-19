using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    [Tooltip("Check if the object has DontDestroyOnLoad property")]
    [SerializeField] bool dontDestroyable = true;
    [SerializeField] bool additiveSingleton = false;
    public static T Instance
    {
        get
        {
            // Avoid returning a null instance of the Singleton.
            // FindObjectOfType has a big cost, but it will be done only once at the start of the game.
            // [!!!] Consider modifying the Awake script order in the Unity Settings.
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (dontDestroyable) DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (additiveSingleton && GetType() != instance.GetType())
            {
                Destroy(Instance.gameObject);
                instance = this as T;
                if (dontDestroyable) DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}
