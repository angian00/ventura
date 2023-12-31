using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ventura.Behaviours
{

    public class MapMouseInputManager : MonoBehaviour
    {
        public UIManager uiManager;
        public Camera targetCamera;
        public BoxCollider2D collider;

        private Vector2Int? _lastPos;


        void Start()
        {
            _lastPos = null;
        }

        void Update()
        {
            var currPos = getTilePos();
            if (currPos != _lastPos)
            {
                uiManager.UpdateTileInfo(currPos);
                _lastPos = currPos;
            }
        }

        private Vector2Int? getTilePos()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            var worldMousePos = targetCamera.ScreenToWorldPoint(mousePos);

            var colliding = Physics2D.OverlapPoint(worldMousePos);
            if (colliding != collider)
                return null;

            return new Vector2Int((int)Math.Round(worldMousePos.x), (int)Math.Round(worldMousePos.y));
        }
    }
}
