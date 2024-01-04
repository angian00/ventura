
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Unity.Events;
using Ventura.Unity.Input;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class ViewManager : MonoBehaviour
    {
        public GameObject inventoryUITemplate;
        public GameObject skillsUITemplate;

        public enum ViewId
        {
            Map,
            Inventory,
            Skills,
            System,
            Popup,
        }

        public Camera mapCamera;
        public Camera secondaryUICamera;
        public Canvas secondaryUICanvas;
        public Camera popupCamera;
        public Canvas popupCanvas;

        public PopupBehaviour popupManager;

        private ViewId? _activeView;
        private ViewId? _mainView;

        public Camera CurrCamera { get => secondaryUICamera.enabled ? secondaryUICamera : mapCamera; }

        private Dictionary<ViewId, AbstractViewInputHandler> _inputHandlers = new();
        public AbstractViewInputHandler? CurrInputHandler { get => _activeView == null ? null : _inputHandlers[(ViewId)_activeView]; }


        private void OnEnable()
        {
            EventManager.UIRequestEvent.AddListener(onUIEvent);
        }

        private void OnDisable()
        {
            EventManager.UIRequestEvent.RemoveListener(onUIEvent);
        }


        void Awake()
        {
            _inputHandlers.Add(ViewId.Map, new MapInputHandler(this));
            _inputHandlers.Add(ViewId.Inventory, new InventoryInputHandler(this));
            _inputHandlers.Add(ViewId.Skills, new SkillsInputHandler(this));
            _inputHandlers.Add(ViewId.System, new SystemMenuInputHandler(this));
            _inputHandlers.Add(ViewId.Popup, new PopupInputHandler(this, popupManager));
        }


        void Start()
        {
            popupManager.viewManager = this;

            resetDefaultView();
        }


        private void onUIEvent(UIRequestData uiRequest)
        {
            if (uiRequest is ResetViewRequest)
            {
                resetDefaultView();
            }
            else if (uiRequest is AskConfirmationRequest)
            {
                var popupRequest = (AskConfirmationRequest)uiRequest;
                ShowPopup(popupRequest.Title, popupRequest.Command);
            }
        }


        private void resetDefaultView()
        {
            foreach (var viewId in DataUtils.EnumValues<ViewId>())
                hideView(viewId);

            EventManager.StatusNotificationEvent.Invoke(null);
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

            EventManager.StatusNotificationEvent.Invoke(null);

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

        public void ShowPopup(string title, SystemCommand command)
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
