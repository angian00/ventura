using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ventura.Unity.Behaviours
{

    public class MapTileBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        private Vector2Int _mapPos;
        public Vector2Int MapPos { set { _mapPos = value; } }

        public MainViewBehaviour mapManager;


        public void OnPointerClick(PointerEventData eventData)
        {
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
