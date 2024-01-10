using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic.Entities;
using Ventura.Unity.ScriptableObjects;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class InventoryItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        private GameItem _gameItem;
        public GameItem GameItem { set { _gameItem = value; } }

        public SpriteConfig spriteConfig;

        [HideInInspector]
        public InventoryViewBehaviour inventoryManager;

        public Image thumbnail;
        public GameObject labelPanel;
        public TextMeshProUGUI labelText;

        void Start()
        {
            thumbnail.sprite = spriteConfig.Get(_gameItem.SpriteId);
            var c = UnityUtils.ColorFromHex(_gameItem.Color);
            if (c != null)
                thumbnail.color = (Color)c;

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
