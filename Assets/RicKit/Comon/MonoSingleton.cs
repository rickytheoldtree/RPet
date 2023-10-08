using UnityEngine;

namespace RicKit.Comon
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        public static T I
        {
            get
            {
                if (instance) return instance;
                new GameObject(typeof(T).Name, typeof(T)).TryGetComponent(out T tempInstance);
                instance = tempInstance;
                return instance;
            }
        }

        public void Awake()
        {
            if (CanAwake()) GetAwake();
        }

        protected abstract void GetAwake();

        private bool CanAwake()
        {
            if (!instance)
            {
                instance = (T)this;
                return true;
            }

            Destroy(gameObject);
            return false;
        }

        public static bool IsNull()
        {
            return !instance;
        }
    }
}