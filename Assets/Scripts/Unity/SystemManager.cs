using UnityEngine;
using Ventura.GameLogic;
using UnityEngine.SceneManagement;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
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
                ViewManager.Instance.NewGame();
                Orchestrator.Instance.NewGame();
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
            DebugUtils.Log("TODO: SystemManager.loadGame()");
        }

        private void saveGame()
        {
            DebugUtils.Log("TODO: SystemManager.saveGame()");
        }
    }
}

