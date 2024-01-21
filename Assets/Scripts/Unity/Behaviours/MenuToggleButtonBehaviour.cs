using UnityEngine;

namespace Ventura.Unity.Behaviours
{

    public class MenuToggleButtonBehaviour : MonoBehaviour
    {
        public GameObject menu;


        public void ToggleMenu()
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
}
