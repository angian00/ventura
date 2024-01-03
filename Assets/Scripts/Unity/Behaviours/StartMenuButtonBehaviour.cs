using UnityEngine;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class StartMenuButtonBehaviour : MonoBehaviour
    {
        public SystemCommand command;


        public void OnButtonClick()
        {
            Debug.Log($"StartMenuButtonBehaviour.OnButtonClick; gameObject: {gameObject}");
            EventManager.SystemCommandRequestEvent.Invoke(command);
        }
    }
}
