using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class StartScreenManager : MonoBehaviour
    {
        public Image SplashImage;
        public float SplashFadeOutDuration;

        void Start()
        {
            SplashImage.CrossFadeAlpha(0.0f, SplashFadeOutDuration, true);
        }


        public void NewGame()
        {
            DebugUtils.Log("Switching to Game Scene");
            SceneManager.LoadScene("Game Scene");
        }

        public void ExitGame()
        {
            DebugUtils.Log("Exiting Game");
            Application.Quit();
        }


        public void LoadGame()
        {
            //TODO: LoadGame
        }

        public void SaveGame()
        {
            //TODO: SaveGame?
        }

    }
}
