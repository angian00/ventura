using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Ventura.Behaviours
{

    public class TileHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UIManager _uiManager;
        private int x;
        private int y;


        void Start()
        {
            x = (int)Math.Round(gameObject.transform.position[0]);
            y = (int)Math.Round(gameObject.transform.position[1]);

            _uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"Mouse is on tile {x}, {y}");
            _uiManager.UpdateTileInfo(new Vector2Int(x, y));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"Mouse is no longer on tile {x}, {y}");
            _uiManager.UpdateTileInfo(null);
        }
    }

}
