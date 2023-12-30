using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Behaviours
{

    public class InventoryUIManager : MonoBehaviour
    {
        public GameObject inventoryItemTemplate;
        
        private Transform _contentRoot;

        private Orchestrator _orch;
        //private TextMeshProUGUI _debugText;



        void Start()
        {
            _orch = Orchestrator.Instance;

            _contentRoot = transform.Find("Content Panel");

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

                var newItem = Instantiate(inventoryItemTemplate);
                newItem.transform.SetParent(_contentRoot, false);

                newItem.SendMessage("Customize", invItem);
            }
        }

    }
}
