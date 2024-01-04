using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ventura.Unity.Behaviours
{

    public class MapTileBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        private Vector2Int _mapPos;
        public Vector2Int MapPos { set { _mapPos = value; } }

        public MainViewBehaviour mapManager;


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemBehaviour.OnButtonClick; gameItem.Name: {gameItem.Name}");

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