using UnityEngine;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class StartMenuButton : MonoBehaviour
    {
        public SystemCommand command;


        public void OnButtonClick()
        {
            Debug.Log($"StartMenuButton.OnButtonClick; gameObject: {gameObject}");
            EventManager.SystemCommandRequestEvent.Invoke(command);
        }
    }
}
