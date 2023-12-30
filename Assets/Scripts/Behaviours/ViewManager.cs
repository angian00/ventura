
using System.Collections.Generic;
using UnityEngine;
using Ventura.Util;

namespace Ventura.Behaviours
{
    //public interface ModalUIManager
    //{
    //    public void UpdateData();
    //}


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

        private const int foregroundDepth = 99;
        private const int hiddenDepth = -99;

        private Dictionary<ViewId, GameObject> _viewTemplates;

        private Camera _modalUICamera;
        private Transform _modalUIRoot;
        //private Dictionary<ViewId, GameObject> _modalUIObjs = new();

        private ViewId _currView;
        private Dictionary<ViewId, KeyboardInputReceiver> _keyboardInputReceivers = new();

        public KeyboardInputReceiver CurrKeyboardReceiver { get => _keyboardInputReceivers[_currView]; }


        void Start()
        {
            _modalUICamera = GameObject.Find("Modal UI Camera").GetComponent<Camera>();
            _modalUIRoot = GameObject.Find("Modal UI Canvas").transform.Find("Instantiation Target");

            _keyboardInputReceivers.Add(ViewId.Map, new MapInputReceiver(this));
            _keyboardInputReceivers.Add(ViewId.Inventory, new InventoryInputReceiver(this));
            _keyboardInputReceivers.Add(ViewId.Skills, new SkillsInputReceiver(this));


            _viewTemplates = new()
            {
                { ViewId.Inventory, inventoryUITemplate },
                { ViewId.Skills,    skillsUITemplate    },
            };

            _currView = ViewId.Map;
        }


        public void SwitchTo(ViewId targetView)
        {
            //SceneManager.LoadScene("Test Modal UI", LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Test Modal UI")); //FIXME: move it where the scene has loaded

            if (_currView == targetView)
                return;

            if (targetView == ViewId.Map)
            {
                _modalUICamera.depth = hiddenDepth;
            }
            else
            {
                _modalUICamera.depth = foregroundDepth;
                //FUTURE: reuse old _modalUIObjs

                //GameObject modalUIObj;
                //lastModalUIObj.SetActive(false);
                //if (_modalUIObjs.ContainsKey(targetView))
                //    modalUIObj = _modalUIObjs[targetView];

                //else {
                //    modalUIObj = Instantiate(inventoryUITemplate);
                //    modalUIObj.transform.SetParent(_modalUIRoot);
                //}
                //_modalUIObjs[targetView] = modalUIObj;
                //modalUIObj.GetComponent<ModalUIManager>().UpdateData(); //too early to call it here anyway


                UnityUtils.RemoveAllChildren(_modalUIRoot);

                var modalUIObj = Instantiate(_viewTemplates[targetView]);
                modalUIObj.transform.SetParent(_modalUIRoot, false);
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
