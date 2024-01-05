using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.Unity.Graphics;

namespace Ventura.Unity.Behaviours
{

    public class InventoryItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        private GameItem _gameItem;
        public GameItem GameItem { set { _gameItem = value; } }

        public InventoryViewBehaviour inventoryManager;

        public Image thumbnail;
        public GameObject labelPanel;
        public TextMeshProUGUI labelText;

        void Start()
        {
            var spriteId = _gameItem is BookItem ? "book" : "item";
            thumbnail.sprite = SpriteCache.Instance.GetSprite(spriteId);
            labelText.text = _gameItem.Label;

            labelPanel.SetActive(false);
        }


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemBehaviour.OnButtonClick; gameItem.Name: {gameItem.Name}");
            inventoryManager.OnItemClick(_gameItem);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"InventoryItemBehaviour.OnPointerEnter; gameItem.Name: {gameItem.Name}");
            labelPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"InventoryItemBehaviour.OnPointerExit; gameItem.Name: {gameItem.Name}");
            labelPanel.SetActive(false);
        }

    }
}
