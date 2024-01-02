using System;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public interface SecondaryUIManager
    {
        public Player PlayerData { set; }
    }

    public class InventoryUIManager : MonoBehaviour, SecondaryUIManager
    {
        [NonSerialized]
        private Player _playerData;
        public Player PlayerData { set => _playerData = value; }

        public GameObject inventoryItemTemplate;
        public Transform _contentRoot;


        void Start()
        {
            updateView();
        }


        void Update()
        {
            if (Orchestrator.Instance.PendingUpdates.Contains(PendingUpdateId.Inventory))
                updateView();
        }

        public void OnItemClick(GameItem gameItem)
        {
            var orch = Orchestrator.Instance;
            var newAction = new UseAction(orch.GameState.Player, gameItem);
            orch.EnqueuePlayerAction(newAction);
        }


        public void updateView()
        {
            Debug.Assert(_playerData.Inventory != null);

            UnityUtils.RemoveAllChildren(_contentRoot);
            foreach (var invItem in _playerData.Inventory.Items)
            {
                DebugUtils.Log($"Found in inventory: {invItem.Name}");

                var newItemObj = Instantiate(inventoryItemTemplate);
                newItemObj.GetComponent<InventoryItemManager>().inventoryManager = this;
                newItemObj.GetComponent<InventoryItemManager>().GameItem = invItem;
                newItemObj.transform.SetParent(_contentRoot, false);
            }
        }

    }
}
