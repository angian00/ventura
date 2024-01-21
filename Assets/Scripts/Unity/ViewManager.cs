using System.Collections.Generic;
using UnityEngine;
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
            else if (uiRequest.command == UIRequest.Command.ShowUIView)
            {
                var viewId = ((ShowUIViewRequest)uiRequest).viewId;
                UnityUtils.ShowUIView(_uiViews[viewId]);
            }
        }

        private void resetDefaultView()
        {
            foreach (var viewId in _uiViews.Keys)
                UnityUtils.HideUIView(_uiViews[viewId]);

            EventManager.Publish(new TextNotification(null));
        }
    }
}
