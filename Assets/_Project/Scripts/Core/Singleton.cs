using UnityEngine;

namespace ByNorth.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T: Component
    {
        public static T Instance { get; private set; } = null;
        
        
        [Header("Scene 전환에서 안 사라지게")]
        [SerializeField] private bool isDontDestroyOnLoad = false;


        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                if (isDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
    }
}
