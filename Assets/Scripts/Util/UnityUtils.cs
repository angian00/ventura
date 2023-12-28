
using UnityEngine;

namespace Ventura.Util
{
    public class UnityUtils
    {
        public static void RemoveAllChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
