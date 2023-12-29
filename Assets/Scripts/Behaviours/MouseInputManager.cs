using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace Ventura.Behaviours
{

    public class MouseInputManager : MonoBehaviour
    {
        private UIManager _uiManager;
        private Camera _targetCamera;
        private BoxCollider2D _collider;

        private Vector2Int? _lastPos;


        void Start()
        {
            _uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
            _targetCamera = GameObject.Find("Map Camera").GetComponent<Camera>();
            _collider = GameObject.Find("Map").GetComponent<BoxCollider2D>();

            _lastPos = null;

        }

        void Update()
        {
            var currPos = getTilePos();
            if (currPos != _lastPos)
            {
                _uiManager.UpdateTileInfo(currPos);
                _lastPos = currPos;
            }
        }

        private Vector2Int? getTilePos()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            var worldMousePos = _targetCamera.ScreenToWorldPoint(mousePos);

            var colliding = Physics2D.OverlapPoint(worldMousePos);
            if (colliding != _collider)
                return null;

            return new Vector2Int((int)Math.Round(worldMousePos.x), (int)Math.Round(worldMousePos.y));
        }
    }
}
