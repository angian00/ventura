using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Util;

namespace Ventura.Behaviours
{

    public class KeyboardInputManager : MonoBehaviour
    {
        private ViewManager _viewManager;
        private Dictionary<KeyControl, float> _keyElapsedTimes = new();
        private Dictionary<KeyControl, bool> _pressedKeys = new();

        [Tooltip("In seconds")]
        public float keyRepeatInitialDelay = 0.5f;
        [Tooltip("In seconds")]
        public float keyRepeatRate = 0.1f;


        void Start()
        {
            _viewManager = GameObject.Find("View Manager").GetComponent<ViewManager>();

            var keyboard = Keyboard.current;
            foreach (var key in keyboard.allKeys)
            {
                _pressedKeys[key] = false;
                _keyElapsedTimes[key] = 0.0f;
            }
        }


        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            foreach (var key in keyboard.allKeys)
            {
                // custom code to perform auto-repeat behaviour for keyboard keys
                bool triggered = false;

                if (!key.isPressed)
                {
                    _pressedKeys[key] = false;
                    _keyElapsedTimes[key] = 0.0f;
                    continue;
                }

                if (!_pressedKeys[key])
                {
                    triggered = true;
                    _pressedKeys[key] = true;
                    _keyElapsedTimes[key] = 0.0f;
                }

                if (_keyElapsedTimes[key] >= keyRepeatInitialDelay)
                {
                    triggered = true;
                    _keyElapsedTimes[key] -= keyRepeatRate;
                }

                _keyElapsedTimes[key] += Time.deltaTime;


                if (triggered)
                {
                    DebugUtils.Log("Key press detected: " + key.displayName);
                    _viewManager.CurrKeyboardReceiver.OnKeyPressed(key);
                }
            }
        }
    }


    public abstract class KeyboardInputReceiver
    {
        protected ViewManager _viewManager;

        public KeyboardInputReceiver(ViewManager viewManager)
        {
            this._viewManager = viewManager;
        }

        public abstract void OnKeyPressed(KeyControl key);

        /**
         * Returns true if key needs no more processing
         */
        protected bool processCommonKey(KeyControl key) {
            var keyboard = Keyboard.current;
            var processed = false;

            if (key == keyboard.iKey)
            {
                _viewManager.Toggle(ViewManager.ViewId.Inventory);
                processed = true;
            }
            else if (key == keyboard.sKey)
            {
                _viewManager.Toggle(ViewManager.ViewId.Skills);
                processed = true;
            }
            else if (key == keyboard.escapeKey)
            {
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
                processed = true;
            }

            return processed;
        }
    }


    public class MapInputReceiver: KeyboardInputReceiver
    {
        public MapInputReceiver(ViewManager viewManager) : base(viewManager) { }

        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("MapInputReceiver.OnKeyPressed");

            if (processCommonKey(key))
                return;

            var keyboard = Keyboard.current;
            var orch = Orchestrator.Instance;
            GameAction? newAction = null;


            int deltaX = 0;
            int deltaY = 0;
            if (key == keyboard.rightArrowKey || key == keyboard.numpad6Key)
                deltaX = 1;
            else if (key == keyboard.leftArrowKey || key == keyboard.numpad4Key)
                deltaX = -1;
            else if (key == keyboard.upArrowKey || key == keyboard.numpad8Key)
                deltaY = 1;
            else if (key == keyboard.downArrowKey || key == keyboard.numpad2Key)
                deltaY = -1;

            else if (key == keyboard.numpad9Key)
            {
                deltaX = 1;
                deltaY = 1;
            }
            else if (key == keyboard.numpad3Key)
            {
                deltaX = 1;
                deltaY = -1;
            }
            else if (key == keyboard.numpad7Key)
            {
                deltaX = -1;
                deltaY = 1;
            }
            else if (key == keyboard.numpad1Key)
            {
                deltaX = -1;
                deltaY = -1;
            }
            if (deltaX != 0 || deltaY != 0)
            {
                newAction = new BumpAction(orch, orch.Player, deltaX, deltaY);

            }
            else if (key == keyboard.numpad5Key)
            {
                newAction = new WaitAction(orch, orch.Player);
            }
            else if (key == keyboard.gKey)
            {
                newAction = new PickupAction(orch, orch.Player);
            }

            else
            {
                //ignore keyPressed
            }


            if (newAction != null)
                orch.EnqueuePlayerAction(newAction);
        }
    }


    public class InventoryInputReceiver : KeyboardInputReceiver
    {
        public InventoryInputReceiver(ViewManager viewManager) : base(viewManager) { }

        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("InventoryInputReceiver.OnKeyPressed");

            if (processCommonKey(key))
                return;

            var keyboard = Keyboard.current;
            var orch = Orchestrator.Instance;
            GameAction? newAction = null;

            if (key == keyboard.uKey)
            {
                //newAction = new UseAction(orch, orch.MapPlayerPos, orch.MapPlayerPos.Inventory.Items[0]); //DEBUG
            }
            else
            {
                //ignore keyPressed
            }


            if (newAction != null)
                orch.EnqueuePlayerAction(newAction);


        }
    }


    public class SkillsInputReceiver : KeyboardInputReceiver
    {
        public SkillsInputReceiver(ViewManager viewManager) : base(viewManager) { }

        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("SkillsInputReceiver.OnKeyPressed");

            if (processCommonKey(key))
                return;

            //do nothing
        }
    }
}
