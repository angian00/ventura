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

        private Image _thumbnail;
        private GameObject _labelPanel;
        private TextMeshProUGUI _labelText;


        void Start()
        {
            _thumbnail = transform.Find("Root/Image").GetComponent<Image>();
            _labelPanel = transform.Find("Root/Label Panel").gameObject;
            _labelText = transform.Find("Root/Label Panel/Label Text").GetComponent<TextMeshProUGUI>();

            _thumbnail.sprite = SpriteCache.Instance.GetSprite(gameItem);
            _labelText.text = gameItem.Name;

            _labelPanel.SetActive(false);
        }


        public void OnButtonClick()
        {
            Debug.Log($"InventoryItemHandler.OnButtonClick; gameItem.Name: {gameItem.Name}");

            var orch = Orchestrator.Instance;
            var newAction = new UseAction(orch, orch.Player, gameItem);
            orch.EnqueuePlayerAction(newAction);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _labelPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _labelPanel.SetActive(false);
        }


    }
}
