using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Entities;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class GameManager : MonoBehaviour
    {
        [NonSerialized]
        private GameState _gameState;
        public GameState GameState { get => _gameState; }

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
            EventManager.Subscribe<ActionRequest>(onActionRequest);
            EventManager.Subscribe<InfoRequest>(onInfoRequest);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<ActionRequest>(onActionRequest);
            EventManager.Unsubscribe<InfoRequest>(onInfoRequest);
        }

        void Update()
        {
            processRound();
        }


        //----------------- EventSystem notification listeners -----------------

        private void onActionRequest(ActionRequest actionRequest)
        {
            _playerActionQueue.Enqueue(actionRequest.actionData);
        }

        private void onInfoRequest(InfoRequest infoRequest)
        {
            if (infoRequest.infoType == InfoType.TileContent)
                onTileInfoRequest((TileInfoRequest)infoRequest);
            //else if (infoRequest is PathfindingRequest)
            //    onPathfindingRequest((PathfindingRequest)uiRequest);
        }


        private void onTileInfoRequest(TileInfoRequest tileInfoRequest)
        {
            var maybePos = tileInfoRequest.pos;
            var gameMap = _gameState.CurrMap;

            TileInfoResponse tileInfoResponse = new TileInfoResponse();

            if (maybePos != null && gameMap.IsInBounds(((Vector2Int)maybePos).x, ((Vector2Int)maybePos).y))
            {
                var pos = (Vector2Int)maybePos;
                tileInfoResponse.pos = pos;

                if (gameMap.Explored[pos.x, pos.y])
                    tileInfoResponse.terrain = gameMap.Terrain[pos.x, pos.y].Label;

                if (gameMap.Visible[pos.x, pos.y])
                {
                    var items = gameMap.GetAllEntitiesAt<GameItem>(pos.x, pos.y);
                    var itemNames = new List<string>();
                    foreach (var item in items)
                    {
                        itemNames.Add(item.Name);
                    }
                    tileInfoResponse.items = itemNames;

                    var s = gameMap.GetAnyEntityAt<Site>(pos.x, pos.y);
                    if (s != null)
                        tileInfoResponse.site = s.Name;

                    var a = gameMap.GetAnyEntityAt<Actor>(pos);
                    if (a != null)
                        tileInfoResponse.actor = a.Name;
                }
            }

            EventManager.Publish(tileInfoResponse);
        }

        //private void onPathfindingRequest(PathfindingRequest uiRequest)
        //{
        //    var pathfinding = new Pathfinding(_gameState.CurrMap.GetBlockedTiles());
        //    var path = pathfinding.FindPathAStar(_gameState.Player.pos, uiRequest.EndPos);
        //    DebugUtils.Log("onPathfindingRequest found path:");
        //    foreach (var pos in path)
        //    {
        //        DebugUtils.Log($"{pos}");
        //    }

        //    EventManager.Publish(new PathfindingUpdateData(path));
        //}


        //------------------------ System Command Execution ------------------------------------------
        private void newGame()
        {
            DebugUtils.Log($"Starting New Game");

            _gameState = new GameState();
            _gameState.NewGame();

            DebugUtils.Log($"Game initialized");

            EventManager.Publish(new TextNotification("Welcome, adventurer!"));
        }

        private void loadGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Loading game from {fullPath}");

            var jsonStr = File.ReadAllText(fullPath);
            _gameState = JsonUtility.FromJson<GameState>(jsonStr);

            _gameState.NotifyEverything();

            EventManager.Publish(new TextNotification("Game loaded"));
        }


        public void SaveGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Saving game to {fullPath}");

            string jsonStr = JsonUtility.ToJson(_gameState);
            File.WriteAllText(fullPath, jsonStr);

            EventManager.Publish(new TextNotification("Game saved"));
        }


        //------------------------ Action Scheduling ------------------------------------------

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
                actionData = ((Monster)actor).ChooseAction(_gameState);
            }

            if (actionData == null)
                return false;

            DebugUtils.Log($"Actor {actor.Name} performs {actionData}");

            var startPos = actor.pos;
            var actionResult = performAction(actor, actionData, _gameState);

            if (actionResult.Success)
            {
                if (actor is Player || _gameState.CurrMap.Visible[startPos.x, startPos.y] || _gameState.CurrMap.Visible[actor.x, actor.y])
                {
                    if (actionResult.Reason != null)
                        EventManager.Publish(new TextNotification(actionResult.Reason));
                }
                return true;
            }
            else
            {
                if (actor is Player || actionResult.IsImportant)
                {
                    EventManager.Publish(new TextNotification(actionResult.Reason, TextNotification.Severity.Warning));
                    DebugUtils.Warning($"{actor.Name} Cannot perform {DataUtils.EnumToStr(actionData.ActionType)}: {actionResult.Reason}");
                }
                return false;
            }
        }


        //------------------------ Action Processing ------------------------------------------

        private ActionResult performAction(Actor actor, ActionData actionData, GameState gameState)
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
                case GameActionType.EnterMapAction:
                    action = new EnterMapAction();
                    break;
                case GameActionType.ExitMapAction:
                    action = new ExitMapAction();
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

