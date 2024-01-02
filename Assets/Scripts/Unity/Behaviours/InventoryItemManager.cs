using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.Unity.Graphics;

namespace Ventura.Unity.Behaviours
{

    public class InventoryItemManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        private GameItem _gameItem;
        public GameItem GameItem { set { _gameItem = value; } }

        public InventoryUIManager inventoryManager;

        public Image thumbnail;
        public GameObject labelPanel;
        public TextMeshProUGUI labelText;

        void Start()
        {
            thumbnail.sprite = SpriteCache.Instance.GetSprite(_gameItem);
            labelText.text = _gameItem.Label;

            labelPanel.SetActive(false);
        }


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemManager.OnButtonClick; gameItem.Name: {gameItem.Name}");
            inventoryManager.OnItemClick(_gameItem);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"InventoryItemManager.OnPointerEnter; gameItem.Name: {gameItem.Name}");
            labelPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"InventoryItemManager.OnPointerExit; gameItem.Name: {gameItem.Name}");
            labelPanel.SetActive(false);
        }

    }
}
