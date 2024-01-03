using Ventura.GameLogic;
using Ventura.Util;
using UnityEngine;
using Ventura.Unity.Events;
using System;
using Ventura.GameLogic.Actions;
using System.Collections.Generic;

namespace Ventura.Unity.Behaviours
{

    public class GameManager : MonoBehaviour
    {
        [NonSerialized]
        private GameState _gameState;
        public GameState GameState { get => _gameState; set { _gameState = value; } }


        private CircularList<Actor> _actorScheduler = new();
        private Queue<ActionData> _playerActionQueue = new();


        void Start()
        {
            //FUTURE: use a character creation scene
            EventManager.SystemCommandRequestEvent.Invoke(SystemCommand.New);
            EventManager.StatusNotificationEvent.Invoke("Welcome, adventurer!");
            //
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


        public void NewGame()
        {
            _gameState = new GameState();
            _gameState.NewGame();
        }


        public void Resume()
        {
            foreach (var a in _gameState.CurrMap.GetAllEntities<Actor>())
                _actorScheduler.Add(a);
        }

        public void Suspend()
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


        //----------------- EventSystem notification listeners -----------------
        private void onActionRequest(ActionData actionRequest)
        {
            _playerActionQueue.Enqueue(actionRequest);
        }

        private void onUIRequest(UIRequest uiRequest)
        {
            //UnityEvents do not automatically handle derived classes for its invocation arguments
            var reqType = uiRequest.GetType();
            if (reqType == typeof(MapTileInfoRequest))
                onInputFeedbackRequest((MapTileInfoRequest)uiRequest);
        }


        private void onInputFeedbackRequest(MapTileInfoRequest uiRequest)
        {
            string tileInfo;
            string entityInfo;

            var pos = uiRequest.TilePos;
            var gameMap = _gameState.CurrMap;

            if (pos == null || !gameMap.IsInBounds(((Vector2Int)pos).x, ((Vector2Int)pos).y))
            {
                tileInfo = "";
                entityInfo = "";
            }
            else
            {
                tileInfo = getTileInfo(gameMap, (Vector2Int)pos);
                entityInfo = getEntityInfo(gameMap, (Vector2Int)pos);
            }

            EventManager.MapInfoUpdateEvent.Invoke(tileInfo, entityInfo);
        }

        //---------------------------------------------------------------------


        private string getTileInfo(GameMap gameMap, Vector2Int pos)
        {
            var res = $"x: {pos.x}, y: {pos.y}";

            if (gameMap.Explored[pos.x, pos.y])
                res += $" - {gameMap.Terrain[pos.x, pos.y].Label}";

            return res;
        }


        private string getEntityInfo(GameMap gameMap, Vector2Int pos)
        {
            if (!gameMap.Visible[pos.x, pos.y])
                return "";

            var a = gameMap.GetAnyEntityAt<Actor>(pos);
            if (a != null)
                return a.Name;

            var s = gameMap.GetAnyEntityAt<Site>(pos.x, pos.y);
            if (s != null)
                return s.Name;

            return "";
        }
    }
}

