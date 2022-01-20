using UnityEngine;

namespace Helpers
{
    public static class ObjectCreator
    {
        public static T Create<T>(string prefabPath, Transform parent = null)
        {
            var obj = (GameObject) Object.Instantiate(Resources.Load(prefabPath));

            if(obj == null) Debug.LogError($"Can't create `{prefabPath}` prefab.");
            if(parent != null) obj.transform.SetParent(parent, false);

            return obj.GetComponent<T>() ?? obj.GetComponentInChildren<T>();
        }
    }
}