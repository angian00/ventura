using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ventura.Unity.Behaviours
{

    public class MapTileManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        private Vector2Int _mapPos;
        public Vector2Int MapPos { set { _mapPos = value; } }

        public MapManager mapManager;


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemManager.OnButtonClick; gameItem.Name: {gameItem.Name}");

            mapManager.OnTileClick(_mapPos);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            mapManager.OnTileMouseEnter(_mapPos);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mapManager.OnTileMouseExit(_mapPos);
        }

    }
}
