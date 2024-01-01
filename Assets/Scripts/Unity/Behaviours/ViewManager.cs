
using System.Collections.Generic;
using UnityEngine;
using Ventura.Util;
using Ventura.Unity.Input;
using UnityEngine.UI;

namespace Ventura.Unity.Behaviours
{
    public class ViewManager : MonoBehaviour
    {
        private static ViewManager _instance;
        public static ViewManager Instance { get => _instance; }


        public GameObject inventoryUITemplate;
        public GameObject skillsUITemplate;

        public enum ViewId
        {
            Map,
            Inventory,
            Skills,
        }

        public Camera mapCamera;
        public Camera modalUICamera;
        //public Transform contentRoot;
        public Canvas modalUICanvas;
        public PopupManager popupManager;

        private Dictionary<ViewId, GameObject> _viewTemplates;
        //private Dictionary<ViewId, GameObject> _modalUIObjs = new();

        private ViewId? _currView;

        public Camera CurrCamera { get => modalUICamera.enabled ? modalUICamera : mapCamera; }

        private Dictionary<ViewId, AbstractViewInputHandler> _inputHandlers = new();
        public AbstractViewInputHandler? CurrInputHandler { get => _currView == null ? null : _inputHandlers[(ViewId)_currView]; }


        void Awake()
        {
            _instance = this;

            _inputHandlers.Add(ViewId.Map, new MapInputHandler(this));
            _inputHandlers.Add(ViewId.Inventory, new InventoryInputHandler(this));
            _inputHandlers.Add(ViewId.Skills, new SkillsInputHandler(this));

            _viewTemplates = new()
            {
                { ViewId.Inventory, inventoryUITemplate },
                { ViewId.Skills,    skillsUITemplate    },
            };
        }


        void Start()
        {
            NewGame();
        }

        public void NewGame()
        {
            SwitchTo(ViewId.Map);
        }


        public void SwitchTo(ViewId targetView)
        {
            if (_currView == targetView)
                return;

            if (targetView == ViewId.Map)
            {
                modalUICamera.enabled = false;
                //manual Raycaster management is apparently needed in spite of Canvas RenderCamera property
                modalUICanvas.GetComponent<GraphicRaycaster>().enabled = false;
            }
            else
            {
                modalUICamera.enabled = true;
                modalUICanvas.GetComponent<GraphicRaycaster>().enabled = true;

                //FUTURE: reuse old _modalUIObjs

                //GameObject modalUIObj;
                //lastModalUIObj.SetActive(false);
                //if (_modalUIObjs.ContainsKey(targetView))
                //    modalUIObj = _modalUIObjs[targetView];

                //else {
                //    modalUIObj = Instantiate(inventoryUITemplate);
                //    modalUIObj.transform.SetParent(contentRoot);
                //}
                //_modalUIObjs[targetView] = modalUIObj;
                //modalUIObj.GetComponent<ModalUIManager>().UpdateData(); //too early to call it here anyway

                var contentRoot = modalUICanvas.transform.Find("Content Root");
                UnityUtils.RemoveAllChildren(contentRoot);

                var modalUIObj = Instantiate(_viewTemplates[targetView]);
                modalUIObj.transform.SetParent(contentRoot, false);
            }

            StatusLineManager.Instance.Clear();
            _currView = targetView;
        }

        public void Toggle(ViewId targetView)
        {
            if (_currView == targetView)
                SwitchTo(ViewId.Map);
            else
                SwitchTo(targetView);
        }

        public void ShowPopup(string title, SystemManager.Command command)
        {
            popupManager.ShowPopup(title, command);
        }

    }
}
