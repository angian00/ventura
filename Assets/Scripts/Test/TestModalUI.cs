using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Ventura.Util;

namespace Ventura.Test
{

    public class TestModalUI : MonoBehaviour
    {
        void Start()
        {
            //ensure the modal ui scene is in foreground
            //gameObject.GetComponent<Camera>().depth = 999; 
        }
    }
}
