using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class GameManager : MonoBehaviour
    {
        [NonSerialized]
        private GameState _gameState;
        public GameState GameState { get => _gameState; }

        private CircularList<Actor> _actorScheduler = new();
        private Queue<ActionData> _playerActionQueue = new();

        private static string? _startStateFile = null;
        public static string StartStateFile { set => _startStateFile = value; }


        //----------------- Unity Lifecycle Callbacks -----------------

        void Start()
        {
            DebugUtils.Log($"GameManager.Start(); active scene: {SceneManager.GetActiveScene().name}");


            if (_startStateFile == null)
                newGame();
            else
                loadGame(_startStateFile);
        }

        private void OnEnable()
        {
            EventManager.ActionRequestEvent.AddListener(onActionRequest);
            EventManager.UIRequestEvent.AddListener(onUIRequest);
        }

        private void OnDisable()
        {
            EventManager.ActionRequestEvent.RemoveListener(onActionRequest);
            EventManager.UIRequestEvent.RemoveListener(onUIRequest);
        }

        void Update()
        {
            processTurn();
        }


        //----------------- EventSystem notification listeners -----------------

        private void onActionRequest(ActionData actionRequest)
        {
            _playerActionQueue.Enqueue(actionRequest);
        }

        private void onUIRequest(UIRequestData uiRequest)
        {
            //UnityEvents do not automatically handle derived classes for its invocation arguments
            var reqType = uiRequest.GetType();
            if (reqType == typeof(MapTileInfoRequest))
                onTileInfoRequest((MapTileInfoRequest)uiRequest);
        }


        private void onTileInfoRequest(MapTileInfoRequest tilePointerRequest)
        {
            var maybePos = tilePointerRequest.TilePos;
            var gameMap = _gameState.CurrMap;

            TileUpdateData tileInfo = new TileUpdateData();

            if (maybePos != null && gameMap.IsInBounds(((Vector2Int)maybePos).x, ((Vector2Int)maybePos).y))
            {
                var pos = (Vector2Int)maybePos;
                tileInfo.Pos = pos;

                if (gameMap.Explored[pos.x, pos.y])
                    tileInfo.Terrain = gameMap.Terrain[pos.x, pos.y].Label;

                if (gameMap.Visible[pos.x, pos.y])
                {
                    var s = gameMap.GetAnyEntityAt<Site>(pos.x, pos.y);
                    if (s != null)
                        tileInfo.Site = s.Name;

                    var a = gameMap.GetAnyEntityAt<Actor>(pos);
                    if (a != null)
                        tileInfo.Actor = a.Name;
                }
            }

            EventManager.GameStateUpdateEvent.Invoke(tileInfo);
        }

        //------------------------ System Command Execution ------------------------------------------
        private void newGame()
        {
            DebugUtils.Log($"Starting New Game");

            _gameState = new GameState();
            _gameState.NewGame();

            DebugUtils.Log($"Game initialized");

            resumeActors();

            EventManager.StatusNotificationEvent.Invoke("Welcome, adventurer!");
        }

        private void loadGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Loading game from {fullPath}");

            var jsonStr = File.ReadAllText(fullPath);
            _gameState = JsonUtility.FromJson<GameState>(jsonStr);

            resumeActors();
            _gameState.NotifyAllEvents();

            EventManager.StatusNotificationEvent.Invoke("Game loaded");
        }


        public void SaveGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Saving game to {fullPath}");

            suspendActors();

            string jsonStr = JsonUtility.ToJson(_gameState);
            File.WriteAllText(fullPath, jsonStr);

            resumeActors();

            EventManager.StatusNotificationEvent.Invoke("Game saved");
        }


        //------------------------ Action Scheduling ------------------------------------------

        private void resumeActors()
        {
            foreach (var a in _gameState.CurrMap.GetAllEntities<Actor>())
                _actorScheduler.Add(a);
        }

        private void suspendActors()
        {
            _actorScheduler.Clear();
        }


        private void processTurn()
        {
            var actor = _actorScheduler.Next();
            if (actor == null)
                return;

            ActionData? actionData = null;
            if (actor is Player)
            {
                if (_playerActionQueue.Count > 0)
                    actionData = _playerActionQueue.Dequeue();
            }
            else
            {
                actionData = actor.ChooseAction();
            }

            if (actionData == null)
                return;

            DebugUtils.Log($"Performing ${DataUtils.EnumToStr(actionData.ActionType)}");

            var actionResult = performAction(actor, actionData);

            if (actionResult.Success)
            {
                if (actionResult.Reason != null)
                    EventManager.StatusNotificationEvent.Invoke(actionResult.Reason, StatusSeverity.Normal);
            }
            else
            {
                EventManager.StatusNotificationEvent.Invoke(actionResult.Reason, StatusSeverity.Warning);
                DebugUtils.Warning($"Cannot perform {DataUtils.EnumToStr(actionData.ActionType)}: {actionResult.Reason}");
            }
        }


        //------------------------ Action Processing ------------------------------------------

        private ActionResult performAction(Actor actor, ActionData actionData)
        {
            GameAction action = null;

            switch (actionData.ActionType)
            {
                case GameActionType.BumpAction:
                    action = new BumpAction();
                    break;
                case GameActionType.UseItemAction:
                    action = new UseItemAction();
                    break;
                default:
                    throw new GameException($"Unsupported ActionType: {DataUtils.EnumToStr(actionData.ActionType)}");
            }

            return action.Perform(actor, actionData, _gameState);
        }

    }
}

