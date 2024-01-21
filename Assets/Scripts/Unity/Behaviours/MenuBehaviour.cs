using UnityEngine;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class MenuBehaviour : MonoBehaviour
    {

        public void ToggleMenu()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void ShowInventoryUI()
        {
            gameObject.SetActive(false);
            EventManager.Publish(new ToggleSecondaryViewRequest(UIRequest.ViewId.Inventory));
        }

        public void ShowSystemUI()
        {
            gameObject.SetActive(false);
            EventManager.Publish(new ToggleSecondaryViewRequest(UIRequest.ViewId.System));
        }
    }
}
