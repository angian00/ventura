using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Ventura.Behaviours
{

    public class TileHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private UIManager uiManager;
        private int x;
        private int y;


        void Start()
        {
            x = (int)Math.Round(gameObject.transform.position[0]);
            y = (int)Math.Round(gameObject.transform.position[1]);

            uiManager = GameObject.Find("UI Container").GetComponent<UIManager>();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"Mouse is on tile {x}, {y}");
            uiManager.UpdateTileInfo($"x={x}, y={y}");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"Mouse is no longer on tile {x}, {y}");
            uiManager.UpdateTileInfo("");
        }
    }

}
