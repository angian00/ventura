using System;
using Ventura.GameLogic.Actions;

namespace Ventura.GameLogic.Components
{
    [Serializable]
    public abstract class Consumable
    {
        protected GameItem _parent;
        public GameItem Parent { set => _parent = value; }

        protected Consumable(GameItem parent)
        {
            this._parent = parent;
        }

        public void Consume()
        {
            var item = _parent;
            var container = item.Parent;

            container.RemoveItem(item);
        }

        public abstract ActionResult Use(Actor actor, ItemAction action);

    }
}

