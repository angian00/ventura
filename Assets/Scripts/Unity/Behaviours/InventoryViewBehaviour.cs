using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class InventoryViewBehaviour : MonoBehaviour
    {
        public GameObject inventoryItemTemplate;
        public Transform contentRoot;


        private void OnEnable()
        {
            EventManager.GameStateUpdateEvent.AddListener(onGameStateUpdated);
        }

        private void OnDisable()
        {
            EventManager.GameStateUpdateEvent.RemoveListener(onGameStateUpdated);
        }

        public void OnItemClick(GameItem gameItem)
        {
            var actionRequestData = new ActionData(GameActionType.UseItemAction);
            actionRequestData.TargetItem = gameItem;

            EventManager.ActionRequestEvent.Invoke(actionRequestData);
        }



        private void onGameStateUpdated(GameStateUpdateData updateData)
        {
            if (!(updateData is ContainerUpdateData))
                return;

            onInventoryChanged(((ContainerUpdateData)updateData).Container);
        }


        private void onInventoryChanged(Container c)
        {
            if (!(c is Inventory))
                return;

            var inv = (Inventory)c;

            if (!(inv.Parent is Player))
                return;

            updateView(inv);
        }


        public void updateView(Inventory inv)
        {
            UnityUtils.RemoveAllChildren(contentRoot);
            foreach (var invItem in inv.Items)
            {
                var newItemObj = Instantiate(inventoryItemTemplate);
                newItemObj.GetComponent<InventoryItemBehaviour>().inventoryManager = this;
                newItemObj.GetComponent<InventoryItemBehaviour>().GameItem = invItem;
                newItemObj.transform.SetParent(contentRoot, false);
            }
        }

    }
}
