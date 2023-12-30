using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;

namespace Ventura.Behaviours
{

    public class InventoryItemHandler : MonoBehaviour
    {
        private GameItem _gameItem;


        void Customize(GameItem gameItem)
        {
            this._gameItem = gameItem;
        }


        public void OnButtonClick()
        {
            Debug.Log($"InventoryItemHandler.OnButtonClick; gameItem.Name: {_gameItem.Name}");

            var orch = Orchestrator.Instance;
            var newAction = new UseAction(orch, orch.Player, _gameItem);
            orch.EnqueuePlayerAction(newAction);
        }
    }
}
