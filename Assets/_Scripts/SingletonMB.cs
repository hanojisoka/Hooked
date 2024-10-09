using UnityEngine;

public abstract class SingletonMB<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private OnDuplicateBehavior onDuplicate;

    /// The only instance of this class (might be null)
    public static T Instance { get; protected set; }

    public enum OnDuplicateBehavior { Reject, Override }
    public bool IsDontDestroyOnLoad;
    public OnDuplicateBehavior OnDuplicate { get => onDuplicate; private set => onDuplicate = value; }

    /// Registers new Instance or self-destructs if one already exists
    protected virtual void Awake()
    {
        if (typeof(T) == typeof(MonoBehaviour))
        {
            Debug.LogError("Cannot create SingletonMB where T = MonoBehaviour");
            OnDestroyInstance();
        }
        else if (Instance == null)
        {
            Instance = GetComponent<T>();
            if (IsDontDestroyOnLoad)
                DontDestroyOnLoad(Instance.gameObject);
        }
        else if (Instance != GetComponent<T>())
        {
            switch (OnDuplicate)
            {
                case OnDuplicateBehavior.Reject:
                    Debug.LogWarning($"Rejected duplicate SingletonMB of type: {typeof(T)}");
                    OnDestroyInstance();
                    break;
                case OnDuplicateBehavior.Override:
                    Debug.Log($"Overriding duplicate SingletonMB of type: {typeof(T)}");
                    (Instance as SingletonMB<T>).OnDestroyInstance();
                    Instance = GetComponent<T>();
                    break;
                default:
                    break;
            }
        }
    }

    /// Registers new Instance or self-destructs if one already exists
    protected virtual void AwakeSilently()
    {
        if (typeof(T) == typeof(MonoBehaviour))
            OnDestroyInstance();
        else if (Instance == null)
            Instance = GetComponent<T>();
        else if (Instance != GetComponent<T>())
            OnDestroyInstance();
    }

    /// Destroys the new instance
    protected virtual void OnDestroyInstance()
    {
        if (Application.isPlaying)
            Destroy(GetComponent<T>());
    }

    /// When instance is destroyed we should clear our reference to it
    protected virtual void OnDestroy()
    {
        if (Instance == GetComponent<T>() || Instance == this)
            Instance = null;
    }

    /// Gets or Creates an instance as necessary.
    /// <returns>The only instance of this class (be it old or new)</returns>
    public static T GetOrCreateInstance() => Instance ? Instance : Instance = new GameObject(nameof(T)).AddComponent<T>();

    /// Gets or Finds an instance as necessary
    /// <returns>The only instance of this class (be it previously known or not)</returns>
    public static T GetOrFindInstance() => Instance ? Instance : Instance = FindObjectOfType<T>();

    public static bool TryGetInstance(out T t) => t = Instance;

    public static bool TryGetOrFindInstance(out T t) => t = GetOrFindInstance();
}
