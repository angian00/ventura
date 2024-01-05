using UnityEngine;
using UnityEngine.SceneManagement;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class SystemManager : MonoBehaviour
    {
        private const string savegameFile = "testSave.json";


        public GameManager gameManager;


        //----------------- Unity Lifecycle Callbacks -----------------

        private void OnEnable()
        {
            EventManager.SystemCommandEvent.AddListener(onSystemCommand);
        }

        private void OnDisable()
        {
            EventManager.SystemCommandEvent.RemoveListener(onSystemCommand);
        }


        //----------------- EventSystem notification listeners -----------------

        private void onSystemCommand(SystemCommand command)
        {
            DebugUtils.Log($"SystemManager.onSystemCommand(); command: {DataUtils.EnumToStr(command)}");

            switch (command)
            {
                case SystemCommand.New:
                    GameManager.StartStateFile = null;
                    SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
                    break;

                case SystemCommand.Exit:
                    exitGame();
                    break;

                case SystemCommand.Load:
                    GameManager.StartStateFile = savegameFile;
                    SceneManager.LoadScene(UnityUtils.GAME_SCENE_NAME);
                    break;

                case SystemCommand.Save:
                    gameManager.SaveGame(savegameFile);
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

    }
}

