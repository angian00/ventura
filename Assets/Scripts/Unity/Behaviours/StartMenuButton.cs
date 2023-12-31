using UnityEngine;

namespace Ventura.Unity.Behaviours
{

    public class StartMenuButton : MonoBehaviour
    {
        public SystemManager.Command command;


        public void OnButtonClick()
        {
            Debug.Log($"StartMenuButton.OnButtonClick; gameObject: {gameObject}");
            SystemManager.Instance.ExecuteCommand(command);
        }
    }
}
