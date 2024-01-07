using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class PopupBehaviour : MonoBehaviour
    {
        private SystemRequest.Command _command;
        public SystemRequest.Command Command { set { _command = value; } }

        public ViewManager viewManager;
        public TextMeshProUGUI titleObj;

        //public Camera popupCamera;
        public GraphicRaycaster raycaster;

        public string Title { set => titleObj.text = value; }

        //private void Awake()
        //{
        //    hide();
        //}

        //public void Show(string title, SystemCommandManager.Command _command)
        //{
        //    titleObj.text = title;
        //    _command = _command;

        //    show();
        //}

        //public void Hide()
        //{

        //}

        public void OnYes()
        {
            Debug.Log($"PopupBehaviour.OnYes()");
            viewManager.HidePopup();

            EventManager.Publish(new SystemRequest(_command));

        }

        public void OnNo()
        {
            Debug.Log($"PopupBehaviour.OnNo()");
            viewManager.HidePopup();
        }

    }
}
