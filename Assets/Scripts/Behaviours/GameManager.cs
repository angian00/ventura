using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic;
using Ventura.Util;


namespace Ventura.Behaviours
{
#nullable enable
    public class GameManager : MonoBehaviour
    {
        private Orchestrator _orch;

        private GameObject _playerObj;
        private GameObject _cameraObj;

        void Start()
        {
            _playerObj = GameObject.Find("Player") as GameObject;
            _cameraObj = GameObject.Find("Game Camera") as GameObject;

            _orch = Orchestrator.GetInstance();
            _orch.NewGame();
        }

        void Update()
        {
            _orch.ProcessTurn();

            if (_orch.UpdatePending)
            {
                //TODO: differentiate repaints
                UpdateMap();
                UpdateGameObjects();
                _orch.UpdatePending = false;
            }

        }


        public void OnKeyPressed(KeyControl key)
        {
            //Messages.Log("OnKeyPressed");

            var keyboard = Keyboard.current;
            Action? newAction = null;


            int deltaX = 0;
            int deltaY = 0;
            if (key == keyboard.rightArrowKey)
                deltaX = 1;
            else if (key == keyboard.leftArrowKey)
                deltaX = -1;
            else if (key == keyboard.upArrowKey)
                deltaY = 1;
            else if (key == keyboard.downArrowKey)
                deltaY = -1;

            if (deltaX != 0 || deltaY != 0)
            {
                Messages.Log("creating BumpAction");

                newAction = new BumpAction(_orch, _orch.Player, deltaX, deltaY);

            } else
            {
                //ignore keyPressed
            }

            if (newAction != null)
                _orch.EnqueuePlayerAction(newAction);
        }


        public void UpdateMap()
        {
            var boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
            boardManager.ClearBoard();
            boardManager.InitBoard(_orch.CurrMap);

            //update ui location info
            string locationInfoStr = "";
            var mapNames = _orch.World.GetStackMapNames();

            for (int i=mapNames.Count-1; i >= 0; i--)
            {
                locationInfoStr += mapNames[i];

                if (i > 0)
                    locationInfoStr += " > ";
            }


            var textObj = GameObject.Find("Location Info");
            textObj.GetComponent<TextMeshProUGUI>().text = locationInfoStr;
        }


        public void UpdateGameObjects() {
            var playerX = _orch.Player.X;
            var playerY = _orch.Player.Y;

            var targetObjPos = _playerObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            _playerObj.transform.position = targetObjPos;

            targetObjPos = _cameraObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            _cameraObj.transform.position = targetObjPos;
        }

    }
}

