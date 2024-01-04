using UnityEngine;
using UnityEngine.UI;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class StartScreenBehaviour : MonoBehaviour
    {
        public Image SplashImage;
        public float SplashFadeOutDuration;

        public SystemMenuBehaviour systemMenu;


        void Start()
        {
            DebugUtils.Log("StartScreenBehaviour.Start()");

            SplashImage.CrossFadeAlpha(0.0f, SplashFadeOutDuration, true);
        }


    }
}
