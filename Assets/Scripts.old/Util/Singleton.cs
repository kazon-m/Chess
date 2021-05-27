using UnityEngine;

namespace Util
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        // ReSharper disable once StaticMemberInGenericType
        private static bool _destroyed;

        public static T Instance
        {
            get
            {
                if(_destroyed || _instance != null) return _instance;

                _instance = FindObjectOfType<T>();

                if(_instance != null) return _instance;

                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();

                return _instance;
            }
        }

        private void OnApplicationQuit()
        {
            _instance = null;
            _destroyed = true;
        }
    }
}