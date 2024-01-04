using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class PopupBehaviour : MonoBehaviour
    {
        private SystemCommand _command;
        public SystemCommand Command { set { _command = value; } }

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

            EventManager.SystemCommandEvent.Invoke(_command);

        }

        public void OnNo()
        {
            Debug.Log($"PopupBehaviour.OnNo()");
            viewManager.HidePopup();
        }


        //private void show()
        //{
        //    raycaster.enabled = true;
        //    popupCamera.enabled = true;
        //}

        //private void hide()
        //{
        //    popupCamera.enabled = false;
        //    raycaster.enabled = false;
        //}

    }
}
