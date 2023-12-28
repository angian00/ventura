using Ventura.GameLogic.Actions;


namespace Ventura.GameLogic.Components
{
    public abstract class Consumable
    {
        protected GameItem _parent;

        protected Consumable(GameItem parent)
        {
            this._parent = parent;
        }

        public abstract ActionResult Use(ItemAction action);

        public void Consume()
        {
            var item = _parent;
            var container = item.Parent;

            container.RemoveItem(item);
        }
    }
}

