
using System.Collections.Generic;
using UnityEngine;
using Ventura.Util;
using Ventura.Unity.Input;

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
        }

        public Camera mapCamera;
        public Camera modalUICamera;
        public Transform modalUIRoot;


        private Dictionary<ViewId, GameObject> _viewTemplates;
        //private Dictionary<ViewId, GameObject> _modalUIObjs = new();

        private ViewId? _currView;

        public Camera CurrCamera { get => modalUICamera.enabled ? modalUICamera : mapCamera; }

        private Dictionary<ViewId, ViewInputHandler> _inputHandlers = new();
        public ViewInputHandler? CurrInputHandler { get => _currView == null ? null : _inputHandlers[(ViewId)_currView]; }


        void Start()
        {
            _inputHandlers.Add(ViewId.Map, new MapInputHandler(this));
            _inputHandlers.Add(ViewId.Inventory, new InventoryInputHandler(this));
            _inputHandlers.Add(ViewId.Skills, new SkillsInputHandler(this));

            _viewTemplates = new()
            {
                { ViewId.Inventory, inventoryUITemplate },
                { ViewId.Skills,    skillsUITemplate    },
            };

            SwitchTo(ViewId.Map);
        }


        public void SwitchTo(ViewId targetView)
        {
            if (_currView == targetView)
                return;

            if (targetView == ViewId.Map)
            {
                modalUICamera.enabled = false;
            }
            else
            {
                modalUICamera.enabled = true;

                //FUTURE: reuse old _modalUIObjs

                //GameObject modalUIObj;
                //lastModalUIObj.SetActive(false);
                //if (_modalUIObjs.ContainsKey(targetView))
                //    modalUIObj = _modalUIObjs[targetView];

                //else {
                //    modalUIObj = Instantiate(inventoryUITemplate);
                //    modalUIObj.transform.SetParent(modalUIRoot);
                //}
                //_modalUIObjs[targetView] = modalUIObj;
                //modalUIObj.GetComponent<ModalUIManager>().UpdateData(); //too early to call it here anyway


                UnityUtils.RemoveAllChildren(modalUIRoot);

                var modalUIObj = Instantiate(_viewTemplates[targetView]);
                modalUIObj.transform.SetParent(modalUIRoot, false);
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
    }
}
