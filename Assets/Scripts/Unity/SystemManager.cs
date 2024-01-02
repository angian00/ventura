using Ventura.GameLogic;
using UnityEngine.SceneManagement;
using Ventura.Util;
using System.IO;
using UnityEngine;
using Ventura.Unity.Behaviours;

namespace Ventura.Unity
{
    public class SystemManager
    {
        public enum Command
        {
            New,
            Exit,
            Load,
            Save,
        }

        private static SystemManager _instance = new SystemManager();
        public static SystemManager Instance { get => _instance; }

        public const string GAME_SCENE_NAME = "Game Scene";
        private const string savegameFile = "testSave.json";


        private SystemManager() { }

        public void ExecuteCommand(Command command)
        {
            switch (command)
            {
                case Command.New:
                    newGame();
                    break;
                case Command.Exit:
                    exitGame();
                    break;
                case Command.Load:
                    loadGame();
                    break;
                case Command.Save:
                    saveGame();
                    break;
            }
        }

        private void newGame()
        {
            DebugUtils.Log("SystemManager.newGame()");
            if (SceneManager.GetActiveScene().name != GAME_SCENE_NAME)
            {
                SceneManager.LoadScene(GAME_SCENE_NAME);
            }
            else
            {
                var gameState = new GameState();
                gameState.NewGame();
                Orchestrator.Instance.GameState = gameState;

                ViewManager.Instance.Reset();
                DebugUtils.Log($"Game initialized");
            }
        }


        private void exitGame()
        {
            DebugUtils.Log("Exiting Game");
            //different calls needed if application is run in Unity editor or as a standalone application
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void loadGame()
        {
            var fullPath = Application.persistentDataPath + "/" + savegameFile;
            DebugUtils.Log($"Loading game from {fullPath}");

            var orch = Orchestrator.Instance;
            orch.Suspend();

            var jsonStr = File.ReadAllText(fullPath);
            orch.GameState = JsonUtility.FromJson<GameState>(jsonStr);
            orch.Resume();

            ViewManager.Instance.Reset();

            DebugUtils.Log($"Game loaded");
        }

        private void saveGame()
        {
            var fullPath = Application.persistentDataPath + "/" + savegameFile;
            DebugUtils.Log($"Saving game to {fullPath}");

            var gameState = Orchestrator.Instance.GameState;
            string jsonStr = JsonUtility.ToJson(gameState);
            File.WriteAllText(fullPath, jsonStr);

            DebugUtils.Log("Game saved");
        }
    }
}

