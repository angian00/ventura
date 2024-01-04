using TMPro;
using UnityEngine;
using static Ventura.Unity.Behaviours.SystemMenuBehaviour;

namespace Ventura.Unity.Behaviours
{

    public class SystemMenuButtonBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI buttonLabel;

        public SystemMenuCommand command;
        public SystemMenuBehaviour menuManager;


        public void OnButtonClick()
        {
            Debug.Log($"SystemMenuButtonBehaviour.OnButtonClick; gameObject: {gameObject}");
            //EventManager.SystemCommandEvent.Invoke(command);
        }
    }
}
