using UnityEngine;

namespace Ventura.Unity.Behaviours
{

    public class CloseButtonBehaviour : MonoBehaviour
    {
        public GameObject closeTarget;

        public void HideUI()
        {
            closeTarget.SetActive(false);
        }
    }
}
