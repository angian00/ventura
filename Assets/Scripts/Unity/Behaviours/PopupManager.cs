using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Graphics;

namespace Ventura.Unity.Behaviours
{

    public class PopupManager : MonoBehaviour
    {
        public ViewManager viewManager;
        public TextMeshProUGUI titleObj;

        public Camera popupCamera;
        public GraphicRaycaster raycaster;

        public SystemManager.Command command;

        public string Title { set => titleObj.text = value; }

        //private void Awake()
        //{
        //    hide();
        //}

        //public void Show(string title, SystemManager.Command command)
        //{
        //    titleObj.text = title;
        //    _command = command;

        //    show();
        //}

        //public void Hide()
        //{

        //}

        public void OnYes()
        {
            Debug.Log($"PopupManager.OnYes()");
            viewManager.HidePopup();

            SystemManager.Instance.ExecuteCommand(command);
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
