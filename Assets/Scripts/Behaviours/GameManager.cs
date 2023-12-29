using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Util;


namespace Ventura.Behaviours
{
#nullable enable
    public class GameManager : MonoBehaviour
    {
        private GameObject _playerObj;
        private GameObject _cameraObj;
        private MapManager _mapManager;

        private Orchestrator _orch;


        private void Start()
        {
            _playerObj = GameObject.Find("Player");
            _cameraObj = GameObject.Find("Map Camera");

            _mapManager = GameObject.Find("Map").GetComponent<MapManager>();

            _orch = Orchestrator.GetInstance();
            _orch.NewGame();
        }

        void Update()
        {
            _orch.ProcessTurn();
            updateScreen();
        }


        private void updateScreen()
        {
            foreach (var pendingType in _orch.PendingUpdates)
            {
                switch (pendingType)
                {
                    case Orchestrator.PendingType.Map:
                        updateMap();
                        break;

                    case Orchestrator.PendingType.Player:
                        updatePlayer();
                        break;
                }
            }

            _orch.PendingUpdates.Clear();
        }


        private void updateMap()
        {
            Messages.Log("GameManager.updateMap()");

            _mapManager.ClearMap();
            _mapManager.InitMap(_orch.CurrMap);

            //update ui location info
            string locationInfoStr = "";
            var mapNames = _orch.World.GetStackMapNames();

            for (int i=mapNames.Count-1; i >= 0; i--)
            {
                locationInfoStr += mapNames[i];

                if (i > 0)
                    locationInfoStr += " > ";
            }

            //FIXME: choose if this goes to UI Manager or UIManager.UpdateTileInfo goes here
            var textObj = GameObject.Find("Location Info");
            textObj.GetComponent<TextMeshProUGUI>().text = locationInfoStr;
        }


        private void updatePlayer() {
            //Messages.Log("GameManager.updatePlayer()");

            var playerX = _orch.Player.x;
            var playerY = _orch.Player.y;

            var targetObjPos = _playerObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            _playerObj.transform.position = targetObjPos;

            targetObjPos = _cameraObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            _cameraObj.transform.position = targetObjPos;

            _mapManager.UpdateFog(_orch.CurrMap);
        }

    }
}

