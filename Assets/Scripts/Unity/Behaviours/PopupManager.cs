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
        public TextMeshProUGUI titleObj;

        public Camera popupCamera;
        public GraphicRaycaster raycaster;

        private SystemManager.Command _command;


        private void Awake()
        {
            hide();
        }

        public void ShowPopup(string title, SystemManager.Command command)
        {
            titleObj.text = title;
            _command = command;

            show();
        }

        public void OnYes()
        {
            Debug.Log($"PopupManager.OnYes()");
            hide();

            SystemManager.Instance.ExecuteCommand(_command);
        }

        public void OnNo()
        {
            Debug.Log($"PopupManager.OnNo()");
            hide();
        }


        private void show()
        {
            raycaster.enabled = true;
            popupCamera.enabled = true;
        }

        private void hide()
        {
            popupCamera.enabled = false;
            raycaster.enabled = false;
        }

    }
}
