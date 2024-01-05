namespace Ventura.GameLogic.Actions
{
    public abstract class ItemAction : GameAction
    {
        public abstract ActionResult Perform(Actor actor, ActionData actionData, GameState _);
    }


    public class DropItemAction : ItemAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState _)
        {
            actionData.CheckActionType(GameActionType.DropItemAction);
            var item = actionData.TargetItem;

            if (actor.Inventory.ContainsItem(item))
            {
                actor.Inventory.RemoveItem(item);
                return new ActionResult(true, $"{actor.Name} drops the {item.Name}");
            }
            else
                return new ActionResult(false, $"{actor.Name} doesn't have that item");
        }
    }

    public class UseItemAction : ItemAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState _)
        {
            actionData.CheckActionType(GameActionType.UseItemAction);
            var item = actionData.TargetItem;

            if (item.Consumable != null)
                return item.Consumable.Use(actor, this);

            else
                return new ActionResult(false, $"the {item.Name} cannot be used");
        }
    }

    public class EquipItemAction : ItemAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState _)
        {
            actionData.CheckActionType(GameActionType.EquipItemAction);
            //if (_item.Equippable != null)
            //{
            //    _actor.Equipment.Toggle(_item);

            //    return new ActionResult(true);
            //}
            //else
            //{
            //    return new ActionResult(false, $"the {_item.Name}  is not equippable");
            //}

            return new ActionResult(false, "TODO: implement EquipAction");
        }
    }




    /** 
     * Pickup an item and add it to the inventory, if there is room for it 
     */
    public class PickupItemAction : ItemAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.PickupItemAction);

            var tileItems = gameState.CurrMap.GetAllEntitiesAt<GameItem>(actor.x, actor.y);
            if (tileItems.Count == 0)
                return new ActionResult(false, "There is nothing here to pick up");

            if (actor.Inventory.IsFull)
                return new ActionResult(false, $"{actor.Name} inventory is full");

            var targetItem = tileItems[0]; //FUTURE: properly support the case of multiple tileItems on the same tile

            targetItem.TransferTo(actor.Inventory);
            return new ActionResult(true, $"{actor.Name} picks up the {targetItem.Label}");
        }
    }


    public class CombineAction : GameAction
    {
        public ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.CombineItemsAction);
            //if (_item1.Combinable != null && _item2.Combinable != null)
            //    return _item1.Combinable.combine(_item2.Combinable);
            //else
            //    return new ActionResult(false, $"the {_item1.Name} and {_item2.Name} cannot be combined");
            return new ActionResult(false, "TODO: implement CombineAction");
        }
    }
}
