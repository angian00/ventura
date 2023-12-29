
using UnityEngine;
using Ventura.GameLogic.Components;
using Ventura.Util;
using static UnityEditor.Progress;

namespace Ventura.GameLogic.Actions
{
    public abstract class ItemAction: GameAction
    {
        protected GameItem _item;
        protected Vector2Int? _targetPos;

        public ItemAction(Orchestrator orch, Actor actor, GameItem item, Vector2Int? targetPos): base(orch, actor) {
            this._item = item;

            if (targetPos != null)
                this._targetPos = targetPos;
            else
                this._targetPos = new Vector2Int(actor.x, actor.y);
    
        }

        /** 
         * Return the actor at this actions destination
         */
        public Actor? targetActor {
            get => _targetPos == null ? null : _orch.CurrMap.GetActorAt(((Vector2Int)_targetPos).x, ((Vector2Int)_targetPos).y);
    
        }
    }


    public class DropAction: ItemAction
    {
        public DropAction(Orchestrator _orch, Actor actor, GameItem item, Vector2Int? targetPos) : base(_orch, actor, item, targetPos) { }

        public override ActionResult Perform()
        {
            if (_actor.Inventory.ContainsItem(_item))
            {
                _actor.Inventory.RemoveItem(_item);
                return new ActionResult(true, $"{_actor.Name} drops the {_item.Name}");
            }
            else
            {
                return new ActionResult(false, $"{_actor.Name} doesn't have that item");
            }
        }
    }

    public class UseAction: ItemAction
    {
        public UseAction(Orchestrator orch, Actor actor, GameItem item, Vector2Int? targetPos=null) : base(orch, actor, item, targetPos) { }

        public override ActionResult Perform()
        {
            if (_item.Consumable != null)
               return _item.Consumable.Use(this);

            else
            {
                return new ActionResult(false, $"the {_item.Name} cannot be used");
            }
        }
    }

    public class EquipAction: ItemAction
    {
        public EquipAction(Orchestrator orch, Actor actor, GameItem item, Vector2Int? targetPos) : base(orch, actor, item, targetPos) { }

        public override ActionResult Perform()
        {
            //if (_item.Equippable != null)
            //{
            //    _actor.Equipment.Toggle(_item);

            //    return new ActionResult(true);
            //}
            //else
            //{
            //    return new ActionResult(false, $"the {_item.Name}  is not equippable");
            //}

            DebugUtils.Log("TODO: EquipAction.Perform()");
            return new ActionResult(false, "TODO: implement EquipAction");
        }
    }




    /** 
     * Pickup an item and add it to the inventory, if there is room for it 
     */
    public class PickupAction : GameAction
    {
        public PickupAction(Orchestrator orch, Actor actor) : base(orch, actor) { }

        public override ActionResult Perform()
        {
            var items = _orch.CurrMap.GetItemsAt(_actor.x, _actor.y);
            if (items.Count == 0)
                return new ActionResult(false, "There is nothing here to pick up");

            if (_actor.Inventory.IsFull)
                return new ActionResult(false, $"{_actor.Name} inventory is full");

            var targetItem = items[0]; //TODO: properly support the case of multiple items on the same tile

            _orch.MoveItemTo(targetItem, _actor.Inventory);
            return new ActionResult(true, $"{_actor.Name} picks up the ${targetItem.Name}");
        }
    }


    public class CombineAction: GameAction
    {
        private GameItem _item1;
        private GameItem _item2;

        public CombineAction(Orchestrator _orch, Actor actor, GameItem item1, GameItem item2): base(_orch, actor) {
            this._item1 = item1;
            this._item2 = item2;
        }

        public override ActionResult Perform()
        {
            //if (_item1.Combinable != null && _item2.Combinable != null)
            //    return _item1.Combinable.combine(_item2.Combinable);
            //else
            //    return new ActionResult(false, $"the {_item1.Name} and {_item2.Name} cannot be combined");
            DebugUtils.Log("TODO: CombineAction.Perform()");
            return new ActionResult(false, "TODO: implement CombineAction");

        }
    }
}
