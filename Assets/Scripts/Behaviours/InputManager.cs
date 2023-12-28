using UnityEngine;
using UnityEngine.InputSystem;
using Ventura.GameLogic;
using Ventura.Util;
using static UnityEngine.GraphicsBuffer;

namespace Ventura.Behaviours
{

    public class InputManager : MonoBehaviour
    {
        private GameManager _gameManager;


        void Start()
        {
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        }


        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            foreach (var key in keyboard.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    Messages.Log("Key press detected: " + key.displayName);

                    _gameManager.OnKeyPressed(key);
                }
            }
        }
    }
}