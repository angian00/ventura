using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.Util;

namespace Ventura.Behaviours
{

    public class InputManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private Dictionary<KeyControl, float> _keyElapsedTimes = new();
        private Dictionary<KeyControl, bool> _pressedKeys = new();

        [Tooltip("In seconds")]
        public float keyRepeatInitialDelay = 0.5f;
        [Tooltip("In seconds")]
        public float keyRepeatRate = 0.1f;


        void Start()
        {
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            
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
                    Messages.Log("Key press detected: " + key.displayName);
                    _gameManager.OnKeyPressed(key);
                }
            }
        }
    }
}
