using UnityEngine;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class MenuBehaviour : MonoBehaviour
    {

        public void ToggleMenu()
        {
            UnityUtils.ToggleUIView(gameObject);
        }

        public void ShowInventoryUI()
        {
            UnityUtils.HideUIView(gameObject);

            EventManager.Publish(new ShowUIViewRequest(UIRequest.ViewId.Inventory));
        }

        public void ShowSystemUI()
        {
            UnityUtils.HideUIView(gameObject);
            EventManager.Publish(new ShowUIViewRequest(UIRequest.ViewId.System));
        }
    }
}
