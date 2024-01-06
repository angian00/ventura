using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Algorithms;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class GameManager : MonoBehaviour
    {
        [NonSerialized]
        private GameState _gameState;
        public GameState GameState { get => _gameState; }

        //private CircularList<Actor> _monsterScheduler = new();
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
            processRound();
        }


        //----------------- EventSystem notification listeners -----------------

        private void onActionRequest(ActionData actionRequest)
        {
            _playerActionQueue.Enqueue(actionRequest);
        }

        private void onUIRequest(UIRequestData uiRequest)
        {
            //UnityEvents do not automatically handle derived classes for its invocation arguments
            if (uiRequest is MapTileInfoRequest)
                onTileInfoRequest((MapTileInfoRequest)uiRequest);
            else if (uiRequest is PathfindingRequest)
                onPathfindingRequest((PathfindingRequest)uiRequest);
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
                    var items = gameMap.GetAllEntitiesAt<GameItem>(pos.x, pos.y);
                    var itemNames = new List<string>();
                    foreach (var item in items)
                    {
                        itemNames.Add(item.GetType().Name);
                    }
                    tileInfo.Items = new ReadOnlyCollection<string>(itemNames);

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

        private void onPathfindingRequest(PathfindingRequest uiRequest)
        {
            var pathfinding = new Pathfinding(_gameState.CurrMap.GetBlockedTiles());
            var path = pathfinding.FindPathAStar(_gameState.Player.pos, uiRequest.EndPos);
            DebugUtils.Log("onPathfindingRequest found path:");
            foreach (var pos in path)
            {
                DebugUtils.Log($"{pos}");
            }
            EventManager.GameStateUpdateEvent.Invoke(new PathfindingUpdateData(path));
        }


        //------------------------ System Command Execution ------------------------------------------
        private void newGame()
        {
            DebugUtils.Log($"Starting New Game");

            _gameState = new GameState();
            _gameState.NewGame();

            DebugUtils.Log($"Game initialized");

            //resumeActors();

            EventManager.StatusNotificationEvent.Invoke("Welcome, adventurer!");
        }

        private void loadGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Loading game from {fullPath}");

            var jsonStr = File.ReadAllText(fullPath);
            _gameState = JsonUtility.FromJson<GameState>(jsonStr);

            //resumeActors();
            _gameState.NotifyAllEvents();

            EventManager.StatusNotificationEvent.Invoke("Game loaded");
        }


        public void SaveGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Saving game to {fullPath}");

            //suspendActors();

            string jsonStr = JsonUtility.ToJson(_gameState);
            File.WriteAllText(fullPath, jsonStr);

            //resumeActors();

            EventManager.StatusNotificationEvent.Invoke("Game saved");
        }


        //------------------------ Action Scheduling ------------------------------------------

        //private void resumeActors()
        //{
        //    foreach (var a in _gameState.CurrMap.GetAllEntities<Actor>())
        //    {
        //        if (!(a is Player))
        //            _monsterScheduler.Add(a);
        //    }
        //}

        //private void suspendActors()
        //{
        //    _monsterScheduler.Clear();
        //}


        private void processRound()
        {
            var playerMakesMove = processTurn(_gameState.Player);
            if (!playerMakesMove)
                return;

            var monsters = _gameState.CurrMap.GetAllEntities<Monster>();
            foreach (var monster in monsters)
                processTurn(monster);
        }

        /**
         * returns true if an action was successfully performed, i.e. the turn was consumed
         */
        private bool processTurn(Actor actor)
        {
            //DebugUtils.Log($"processTurn({actor.Name})");

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
                return false;

            DebugUtils.Log($"Actor {actor.Name} performs {actionData}");

            var actionResult = performAction(actor, actionData);

            if (actionResult.Success)
            {
                //FUTURE: improve status notification filtering
                if (actionResult.Reason != null)
                    EventManager.StatusNotificationEvent.Invoke(actionResult.Reason, StatusSeverity.Normal);
                return true;
            }
            else
            {
                //if (actor is Player) //FUTURE: improve status notification filtering
                EventManager.StatusNotificationEvent.Invoke(actionResult.Reason, StatusSeverity.Warning);
                DebugUtils.Warning($"{actor.Name} Cannot perform {DataUtils.EnumToStr(actionData.ActionType)}: {actionResult.Reason}");
                return false;
            }
        }


        //------------------------ Action Processing ------------------------------------------

        private ActionResult performAction(Actor actor, ActionData actionData)
        {
            GameAction action = null;

            switch (actionData.ActionType)
            {
                case GameActionType.WaitAction:
                    action = new WaitAction();
                    break;
                case GameActionType.BumpAction:
                    action = new BumpAction();
                    break;
                case GameActionType.UseItemAction:
                    action = new UseItemAction();
                    break;
                case GameActionType.PickupItemAction:
                    action = new PickupItemAction();
                    break;
                default:
                    throw new GameException($"Unsupported ActionType: {DataUtils.EnumToStr(actionData.ActionType)}");
            }

            return action.Perform(actor, actionData, _gameState);
        }

    }
}

