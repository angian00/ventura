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


    }
}
