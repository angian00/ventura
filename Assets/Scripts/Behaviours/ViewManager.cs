
using System.Collections.Generic;
using UnityEngine;

namespace Ventura.Behaviours
{

    public class ViewManager : MonoBehaviour
    {
        private const int foregroundDepth = 99;
        private const int hiddenDepth = 0;

        public enum ViewId
        {
            Map,
            Inventory,
        }

        private Camera _modalUICamera;
        private ViewId _currView;
        private Dictionary<ViewId, KeyboardInputReceiver> _keyboardInputReceivers = new();

        public KeyboardInputReceiver CurrKeyboardReceiver { get => _keyboardInputReceivers[_currView]; }


        void Start()
        {
            _modalUICamera = GameObject.Find("Modal UI Camera").GetComponent<Camera>();

            _keyboardInputReceivers.Add(ViewId.Map, new MapInputReceiver(this));
            _keyboardInputReceivers.Add(ViewId.Inventory, new InventoryInputReceiver(this));

            _currView = ViewId.Map;
        }


        public void SwitchTo(ViewId targetView)
        {
            //SceneManager.LoadScene("Test Modal UI", LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Test Modal UI")); //FIXME: move it where the scene has loaded

            if (_currView == targetView)
                return;

            if (targetView == ViewId.Map)
                _modalUICamera.depth = hiddenDepth;
            else
                _modalUICamera.depth = foregroundDepth;

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
