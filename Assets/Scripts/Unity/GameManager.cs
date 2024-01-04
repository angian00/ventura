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

        private static GameManager _instance;

        private CircularList<Actor> _actorScheduler = new();
        private Queue<ActionData> _playerActionQueue = new();

        private const string savegameFile = "testSave.json";

        private bool _gameScenePresent = false;
        private SystemCommand? _pendingCommand = null;


        //----------------- Unity Lifecycle Callbacks -----------------

        void Awake()
        {
            DebugUtils.Log($"GameManager.Awake(); HashCode: {GetHashCode()}; active scene: {SceneManager.GetActiveScene().name} ");

            _gameScenePresent = (SceneManager.GetActiveScene().name == UnityUtils.GAME_SCENE_NAME);
        }


        void Start()
        {
            DebugUtils.Log($"GameManager.Start(); HashCode: {GetHashCode()}; active scene: {SceneManager.GetActiveScene().name}");

            if (_instance != null)
            {
                DebugUtils.Log($"Destroying gameObject for HashCode: {GetHashCode()}");

                Destroy(gameObject);
                return;
            }

            //Unity "singleton" (DontDestroyOnLoad) pattern
            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (_gameScenePresent)
            {
                //GameScene is run standalone: trigger a new game
                EventManager.SystemCommandEvent.Invoke(SystemCommand.New);
            }
            else
            {
                SceneManager.sceneLoaded += onSceneLoaded;
            }
        }

        private void OnEnable()
        {
            EventManager.SystemCommandEvent.AddListener(onSystemCommand);
            EventManager.ActionRequestEvent.AddListener(onActionRequest);
            EventManager.UIRequestEvent.AddListener(onUIRequest);
        }

        private void OnDisable()
        {
            EventManager.SystemCommandEvent.RemoveListener(onSystemCommand);
            EventManager.ActionRequestEvent.RemoveListener(onActionRequest);
            EventManager.UIRequestEvent.RemoveListener(onUIRequest);
        }

        private void onSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == UnityUtils.GAME_SCENE_NAME)
                _gameScenePresent = true;

            DebugUtils.Log($"GameManager.onSceneLoaded(); HashCode: {GetHashCode()}");
            completeCommand((SystemCommand)_pendingCommand);
            _pendingCommand = null;
        }

        void Update()
        {
            processTurn();
        }


        //----------------- EventSystem notification listeners -----------------

        private void onSystemCommand(SystemCommand command)
        {
            DebugUtils.Log($"GameManager.onSystemCommand(); HashCode: {GetHashCode()}; command: {DataUtils.EnumToStr(command)}");

            switch (command)
            {
                case SystemCommand.New:
                    executeNewGame();
                    break;
                case SystemCommand.Exit:
                    executeExitGame();
                    break;
                case SystemCommand.Load:
                    executeLoadGame();
                    break;
                case SystemCommand.Save:
                    executeSaveGame();
                    break;
            }
        }

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

        //------------------------ System Commands ------------------------------------------

        private void completeCommand(SystemCommand command)
        {
            DebugUtils.Log($"GameManager.completeCommand(); HashCode: {GetHashCode()}; command: {DataUtils.EnumToStr(command)}");

            if (command == SystemCommand.New)
            {
                _gameState = new GameState();
                _gameState.NewGame();

                DebugUtils.Log($"Game initialized");
                EventManager.StatusNotificationEvent.Invoke("Welcome, adventurer!");
            }
            else if (command == SystemCommand.Load)
            {
                var fullPath = Application.persistentDataPath + "/" + savegameFile;
                DebugUtils.Log($"Loading game from {fullPath}");

                var jsonStr = File.ReadAllText(fullPath);
                _gameState = JsonUtility.FromJson<GameState>(jsonStr);

                _gameState.NotifyAllEvents();
                EventManager.StatusNotificationEvent.Invoke("Game loaded");
            }
            else
            {
                throw new GameException($"System Command {DataUtils.EnumToStr(command)} needs no differed completion");
            }

            resumeActors();
            EventManager.UIRequestEvent.Invoke(new ResetViewRequest());
        }


        private void executeNewGame()
        {
            DebugUtils.Log("Creating New Game");

            if (_gameScenePresent)
            {
                suspendActors();
                completeCommand(SystemCommand.New);
            }
            else
            {
                _pendingCommand = SystemCommand.New;
                SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
            }
        }


        private void executeExitGame()
        {
            DebugUtils.Log("Exiting Game");

            //different calls needed if application is run in Unity editor or as a standalone application
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void executeLoadGame()
        {
            if (_gameScenePresent)
            {
                suspendActors();
                completeCommand(SystemCommand.Load);
            }
            else
            {
                _pendingCommand = SystemCommand.Load;
                SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
            }
        }


        private void executeSaveGame()
        {
            var fullPath = Application.persistentDataPath + "/" + savegameFile;
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

