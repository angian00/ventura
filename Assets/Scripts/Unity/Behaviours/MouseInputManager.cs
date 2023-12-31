using UnityEngine;
using UnityEngine.InputSystem;
using Ventura.Unity.Behaviours;
using Ventura.Util;

namespace Ventura.Unity.Input
{

    public class MouseInputManager : MonoBehaviour
    {
        public ViewManager viewManager;


        private Camera? _lastCamera;
        private Vector2? _lastMousePos;


        void Update()
        {
            if (Mouse.current == null)
                return;

            var camera = viewManager.CurrCamera;
            var mousePos = Mouse.current.position.ReadValue();

            //check if mouse is inside current viewport
            var viewportPos = camera.ScreenToViewportPoint(mousePos);
            if (viewportPos.x < 0.0f || viewportPos.x >= 1.0f || viewportPos.y < 0.0f || viewportPos.y >= 1.0f)
                return;

            //check if mouse has moved
            if (camera == _lastCamera && mousePos == _lastMousePos)
                return;

            _lastCamera = camera;
            _lastMousePos = mousePos;
            
            viewManager.CurrInputHandler.OnMouseMove(mousePos, camera);
        }
    }
}
