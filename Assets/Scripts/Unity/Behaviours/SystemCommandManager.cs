using Ventura.GameLogic;
using UnityEngine.SceneManagement;
using Ventura.Util;
using System.IO;
using UnityEngine;
using Ventura.Unity.Behaviours;
using Ventura.Unity.Events;

namespace Ventura.Unity
{
    public class SystemCommandManager: MonoBehaviour
    {
        public const string GAME_SCENE_NAME = "Game Scene";
        private const string savegameFile = "testSave.json";


        private void OnEnable()
        {
            EventManager.SystemCommandRequestEvent.AddListener(onSystemCommand);
        }

        private void OnDisable()
        {
            EventManager.SystemCommandRequestEvent.RemoveListener(onSystemCommand);
        }


        private void onSystemCommand(SystemCommand command)
        {
            switch (command)
            {
                case SystemCommand.New:
                    newGame();
                    break;
                case SystemCommand.Exit:
                    exitGame();
                    break;
                case SystemCommand.Load:
                    loadGame();
                    break;
                case SystemCommand.Save:
                    saveGame();
                    break;
            }
        }

        private void newGame()
        {
            DebugUtils.Log("SystemCommandManager.newGame()");
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

