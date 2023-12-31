using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Graphics;

namespace Ventura.Unity.Behaviours
{

    public class InventoryItemManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameItem gameItem;

        public Image thumbnail;
        public GameObject labelPanel;
        public TextMeshProUGUI labelText;


        void Start()
        {
            thumbnail.sprite = SpriteCache.Instance.GetSprite(gameItem);
            labelText.text = gameItem.Label;

            labelPanel.SetActive(false);
        }


        public void OnButtonClick()
        {
            //Debug.Log($"InventoryItemManager.OnButtonClick; gameItem.Name: {gameItem.Name}");

            var orch = Orchestrator.Instance;
            var newAction = new UseAction(orch, orch.Player, gameItem);
            orch.EnqueuePlayerAction(newAction);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"InventoryItemManager.OnPointerEnter; gameItem.Name: {gameItem.Name}");
            labelPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"InventoryItemManager.OnPointerExit; gameItem.Name: {gameItem.Name}");
            labelPanel.SetActive(false);
        }

    }
}
