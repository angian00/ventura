using TMPro;
using UnityEngine;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class PopupBehaviour : MonoBehaviour
    {
        private SystemRequest.Command _command;

        public TextMeshProUGUI titleObj;


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
                UnityUtils.ShowUIView(gameObject);
            }
        }

        public void OnYes()
        {
            Debug.Log($"PopupBehaviour.OnYes()");
            UnityUtils.HideUIView(gameObject);

            EventManager.Publish(new SystemRequest(_command));
        }

        public void OnNo()
        {
            Debug.Log($"PopupBehaviour.OnNo()");
            UnityUtils.HideUIView(gameObject);
        }

    }
}
