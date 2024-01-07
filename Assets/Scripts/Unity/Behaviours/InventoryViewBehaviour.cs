using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.GameLogic.Entities;
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
            EventManager.Subscribe<EntityUpdate>(onActorUpdate);

        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<EntityUpdate>(onActorUpdate);
        }

        public void OnItemClick(GameItem gameItem)
        {
            var actionData = new ActionData(GameActionType.UseItemAction);
            actionData.TargetItem = gameItem;

            EventManager.Publish(new ActionRequest(actionData));
        }



        private void onActorUpdate(EntityUpdate updateData)
        {
            if (!(updateData.entity is Player) || !(updateData.type == EntityUpdate.Type.Changed))
                return;

            var inventoryData = ((Player)updateData.entity).Inventory;
            updateView(inventoryData);
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
