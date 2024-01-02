using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ventura.Unity.Behaviours
{

    public class PopupManager : MonoBehaviour
    {
        private SystemManager.Command _command;
        public SystemManager.Command Command { set { _command = value; } }

        public ViewManager viewManager;
        public TextMeshProUGUI titleObj;

        public Camera popupCamera;
        public GraphicRaycaster raycaster;

        public string Title { set => titleObj.text = value; }

        //private void Awake()
        //{
        //    hide();
        //}

        //public void Show(string title, SystemManager.Command _command)
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
            Debug.Log($"PopupManager.OnYes()");
            viewManager.HidePopup();

            SystemManager.Instance.ExecuteCommand(_command);
        }

        public void OnNo()
        {
            Debug.Log($"PopupManager.OnNo()");
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
