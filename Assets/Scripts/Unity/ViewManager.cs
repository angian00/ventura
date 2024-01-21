using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class ViewManager : MonoBehaviour
    {
        public GameObject inventoryUI;
        public GameObject systemUI;

        private Dictionary<UIRequest.ViewId, GameObject> _uiViews;

        private void OnEnable()
        {
            EventManager.Subscribe<UIRequest>(onUIRequest);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<UIRequest>(onUIRequest);
        }

        void Awake()
        {
            _uiViews = new();
            _uiViews.Add(UIRequest.ViewId.Inventory, inventoryUI);
            _uiViews.Add(UIRequest.ViewId.System, systemUI);
        }

        void Start()
        {
            resetDefaultView();
        }


        private void onUIRequest(UIRequest uiRequest)
        {
            if (uiRequest.command == UIRequest.Command.ResetView)
            {
                resetDefaultView();
            }
            else if (uiRequest.command == UIRequest.Command.ToggleSecondaryView)
            {
                toggleView(((ToggleSecondaryViewRequest)uiRequest).viewId);
            }
        }

        private void resetDefaultView()
        {
            foreach (var viewId in _uiViews.Keys)
            {
                _uiViews[viewId].SetActive(false);
            }
            EventManager.Publish(new TextNotification(null));
        }

        private void toggleView(UIRequest.ViewId viewId)
        {
            var viewObj = _uiViews[viewId];
            if (viewObj == null)
                throw new GameException($"viewObj not found for viewId {DataUtils.EnumToStr(viewId)}");

            viewObj.SetActive(!viewObj.activeSelf);
        }
    }
}
