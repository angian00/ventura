using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ventura.GameLogic;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class SystemCommandManager : MonoBehaviour
    {
        private const string savegameFile = "testSave.json";

        public GameManager gameStateManager;


        private void OnEnable()
        {
            EventManager.SystemCommandEvent.AddListener(onSystemCommand);
        }

        private void OnDisable()
        {
            EventManager.SystemCommandEvent.RemoveListener(onSystemCommand);
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
            if (SceneManager.GetActiveScene().name != UnityUtils.GAME_SCENE_NAME)
            {
                SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
            }
            else
            {
                gameStateManager.Suspend();
                gameStateManager.NewGame();
                gameStateManager.Resume();

                EventManager.UIRequestEvent.Invoke(new ResetViewRequest());
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

            gameStateManager.Suspend();

            var jsonStr = File.ReadAllText(fullPath);
            var gameState = JsonUtility.FromJson<GameState>(jsonStr);
            gameStateManager.GameState = gameState;

            gameStateManager.Resume();

            EventManager.UIRequestEvent.Invoke(new ResetViewRequest());

            DebugUtils.Log($"Game loaded");
        }

        private void saveGame()
        {
            var fullPath = Application.persistentDataPath + "/" + savegameFile;
            DebugUtils.Log($"Saving game to {fullPath}");

            gameStateManager.Suspend();

            var gameState = gameStateManager.GameState;
            string jsonStr = JsonUtility.ToJson(gameState);
            File.WriteAllText(fullPath, jsonStr);

            gameStateManager.Resume();

            DebugUtils.Log("Game saved");
        }
    }
}

