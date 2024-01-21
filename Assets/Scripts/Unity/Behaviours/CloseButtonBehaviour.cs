using UnityEngine;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class CloseButtonBehaviour : MonoBehaviour
    {
        public GameObject targetObj;

        public void HideUI()
        {
            UnityUtils.HideUIView(targetObj);
        }
    }
}
