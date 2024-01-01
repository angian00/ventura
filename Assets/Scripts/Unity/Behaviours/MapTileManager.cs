using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Graphics;

namespace Ventura.Unity.Behaviours
{

    public class MapTileManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public MapManager mapManager;
        public Vector2Int mapPos;


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemManager.OnButtonClick; gameItem.Name: {gameItem.Name}");

            mapManager.OnTileClick(mapPos);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            mapManager.OnTileMouseEnter(mapPos);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mapManager.OnTileMouseExit(mapPos);
        }

    }
}
