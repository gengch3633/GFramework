using UnityEngine;

namespace GameFramework
{

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static bool IsQuiting = false;

        public static bool IsInited { get { return _instance != null; } }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (IsQuiting == true)
                        return _instance;

                    _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                        _instance = new GameObject($"MonoSingleton_{typeof(T).Name}", typeof(T)).GetComponent<T>();
                    _instance.HandleAwake();
                }

                DontDestroyOnLoad(_instance);
                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                _instance.HandleAwake();
            }
        }

        protected virtual void HandleAwake() { }

        private void OnApplicationQuit()
        {
            HandleApplicationQuit();

            _instance = null;
            IsQuiting = true;
        }

        protected virtual void HandleApplicationQuit() { }

        private static T _instance = null;
    }
}