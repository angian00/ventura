
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
            Popup,
        }

        public Camera mapCamera;
        public Camera secondaryUICamera;
        public Canvas secondaryUICanvas;
        public Camera popupCamera;
        public Canvas popupCanvas;

        public PopupManager popupManager;

        private Dictionary<ViewId, GameObject> _secondaryUITemplates;

        private ViewId? _activeView;
        private ViewId? _mainView;

        public Camera CurrCamera { get => secondaryUICamera.enabled ? secondaryUICamera : mapCamera; }

        private Dictionary<ViewId, AbstractViewInputHandler> _inputHandlers = new();
        public AbstractViewInputHandler? CurrInputHandler { get => _activeView == null ? null : _inputHandlers[(ViewId)_activeView]; }


        void Awake()
        {
            _instance = this;

            _inputHandlers.Add(ViewId.Map, new MapInputHandler(this));
            _inputHandlers.Add(ViewId.Inventory, new InventoryInputHandler(this));
            _inputHandlers.Add(ViewId.Skills, new SkillsInputHandler(this));
            _inputHandlers.Add(ViewId.Popup, new PopupInputHandler(this, popupManager));

            _secondaryUITemplates = new()
            {
                { ViewId.Inventory, inventoryUITemplate },
                { ViewId.Skills,    skillsUITemplate    },
            };
        }


        void Start()
        {
            popupManager.viewManager = this;
            Reset();
        }


        public void Reset()
        {
            foreach (var viewId in DataUtils.EnumValues<ViewId>())
                hideView(viewId);

            StatusLineManager.Instance.Clear();
            _activeView = ViewId.Map;
            _mainView = ViewId.Map;
        }


        public void SwitchTo(ViewId targetView)
        {
            if (_activeView == targetView)
                return;

            if (_activeView != null)
                hideView((ViewId)_activeView);

            showView(targetView);

            StatusLineManager.Instance.Clear();
            _activeView = targetView;
            _mainView = targetView;
        }

        public void Toggle(ViewId targetView)
        {
            if (_activeView == targetView)
                SwitchTo(ViewId.Map);
            else
                SwitchTo(targetView);
        }

        public void ShowPopup(string title, SystemManager.Command command)
        {
            popupManager.Title = title;
            popupManager.Command = command;
            showView(ViewId.Popup);

            _activeView = ViewId.Popup;
        }

        public void HidePopup()
        {
            hideView(ViewId.Popup);
            _activeView = _mainView;
        }

        private void showView(ViewId viewId)
        {
            if (viewId == ViewId.Map)
            {
                //do nothing: Map view is always present in the background
                return;
            }

            if (viewId == ViewId.Popup)
            {
                popupCamera.enabled = true;
                popupCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            }
            else
            {
                secondaryUICamera.enabled = true;
                secondaryUICanvas.GetComponent<GraphicRaycaster>().enabled = true;

                var uiObjName = DataUtils.EnumToStr(viewId);
                var uiObj = secondaryUICanvas.transform.Find(uiObjName);
                uiObj.SetAsLastSibling();
            }
        }


        private void hideView(ViewId viewId)
        {
            if (viewId == ViewId.Map)
            {
                //do nothing: Map view is always present in the background
                return;
            }

            if (viewId == ViewId.Popup)
            {
                popupCamera.enabled = false;
                popupCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            }
            else
            {
                secondaryUICamera.enabled = false;
                secondaryUICanvas.GetComponent<GraphicRaycaster>().enabled = false;
            }
        }
    }
}
