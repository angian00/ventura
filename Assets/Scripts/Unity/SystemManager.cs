using UnityEngine;
using UnityEngine.SceneManagement;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class SystemManager : MonoBehaviour
    {
        private const string savegameFile = "testSave.json";
        private const int restartDelaySeconds = 2;

        public GameManager gameManager;


        //----------------- Unity Lifecycle Callbacks -----------------

        private void OnEnable()
        {
            EventManager.Subscribe<SystemRequest>(onSystemCommand);

        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<SystemRequest>(onSystemCommand);
        }


        //----------------- EventSystem notification listeners -----------------

        private void onSystemCommand(SystemRequest commandRequest)
        {
            var command = commandRequest.command;
            DebugUtils.Log($"SystemManager.onSystemCommand(); command: {DataUtils.EnumToStr(command)}");

            switch (command)
            {
                case SystemRequest.Command.New:
                    GameManager.StartStateFile = null;
                    SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
                    break;

                case SystemRequest.Command.Exit:
                    exitGame();
                    break;

                case SystemRequest.Command.Load:
                    GameManager.StartStateFile = savegameFile;
                    SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
                    break;

                case SystemRequest.Command.Save:
                    gameManager.SaveGame(savegameFile);
                    break;

                case SystemRequest.Command.GameOver:
                    Invoke("restartGame", restartDelaySeconds);
                    break;

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

        private void restartGame()
        {
            SceneManager.LoadScene(UnityUtils.START_SCENE_NAME);
        }
    }
}

