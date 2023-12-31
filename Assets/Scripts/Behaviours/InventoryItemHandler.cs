using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Graphics;

namespace Ventura.Behaviours
{

    public class InventoryItemHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameItem gameItem;

        public Image thumbnail;
        public GameObject labelPanel;
        public TextMeshProUGUI labelText;


        void Start()
        {
            thumbnail.sprite = SpriteCache.Instance.GetSprite(gameItem);
            labelText.text = gameItem.Name;

            labelPanel.SetActive(false);
        }


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemHandler.OnButtonClick; gameItem.Name: {gameItem.Name}");

            var orch = Orchestrator.Instance;
            var newAction = new UseAction(orch, orch.Player, gameItem);
            orch.EnqueuePlayerAction(newAction);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            labelPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            labelPanel.SetActive(false);
        }

    }
}
