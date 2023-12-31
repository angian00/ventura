using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class InventoryUIManager : MonoBehaviour
    {
        public GameObject inventoryItemTemplate;
        public Transform _contentRoot;

        private Orchestrator _orch;


        void Start()
        {
            _orch = Orchestrator.Instance;

            updateData();
        }


        void Update()
        {
            if (PendingUpdates.Instance.Contains(PendingUpdateId.Inventory))
                updateData();
        }

        private void updateData()
        {
            var inventory = _orch.Player.Inventory;
            if (inventory == null)
            {
                DebugUtils.Error("No inventory found in player");
                return;
            }

            UnityUtils.RemoveAllChildren(_contentRoot);

            foreach (var invItem in inventory.Items)
            {
                DebugUtils.Log($"Found in inventory: {invItem.Name}");

                var newItemObj = Instantiate(inventoryItemTemplate);
                newItemObj.GetComponent<InventoryItemManager>().gameItem = invItem;
                newItemObj.transform.SetParent(_contentRoot, false);
            }
        }

    }
}
