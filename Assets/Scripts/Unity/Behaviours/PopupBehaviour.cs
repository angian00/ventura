using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class PopupBehaviour : MonoBehaviour
    {
        private SystemRequest.Command _command;

        public TextMeshProUGUI titleObj;

        //public Camera popupCamera;
        public GraphicRaycaster raycaster;


        private void OnEnable()
        {
            EventManager.Subscribe<UIRequest>(onUIRequest);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<UIRequest>(onUIRequest);
        }

        //private void Awake()
        //{
        //    hide();
        //}


        private void onUIRequest(UIRequest uiRequest)
        {
            if (uiRequest.command == UIRequest.Command.AskYesNo)
            {
                var askYesNoRequest = (AskYesNoRequest)uiRequest;
                titleObj.text = askYesNoRequest.title;
                _command = askYesNoRequest.systemCommand;
                gameObject.SetActive(true);
            }
        }

        public void OnYes()
        {
            Debug.Log($"PopupBehaviour.OnYes()");
            gameObject.SetActive(false);

            EventManager.Publish(new SystemRequest(_command));
        }

        public void OnNo()
        {
            Debug.Log($"PopupBehaviour.OnNo()");
            gameObject.SetActive(false);

        }

    }
}
