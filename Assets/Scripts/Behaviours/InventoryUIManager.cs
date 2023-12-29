
using TMPro;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Behaviours
{

    public class InventoryUIManager : MonoBehaviour, ModalUIManager
    {
        private Orchestrator _orch;
        private TextMeshProUGUI _debugText;

        void Start()
        {
            _orch = Orchestrator.GetInstance();
            _debugText = transform.Find("Debug Text").GetComponent<TextMeshProUGUI>();

            UpdateData();
        }

        public void UpdateData()
        {
            var inventory = _orch.Player.Inventory;
            if (inventory == null)
            {
                DebugUtils.Error("No inventory found in player");
                return;
            }

            var msg = "";
            foreach (var invItem in inventory.Items)
            {
                DebugUtils.Log($"Found in inventory: {invItem.Name}");

                msg += invItem.Name;
                msg += "\n";
            }

            _debugText.text = msg;
        }

    }
}
