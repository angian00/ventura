

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ventura.GameLogic.Components
{
    public class Inventory : Container
    {
        protected Actor _parent;
        public Actor Parent { get { return _parent; } }

        private List<GameItem> _items = new();

        private int _maxSize;

        public bool IsFull { get => (_items.Count == _maxSize); }

        public Inventory(Actor parent, int maxSize)
        {
            _parent = parent;
            _maxSize = maxSize;
        }

        public ReadOnlyCollection<GameItem> Items { get => new ReadOnlyCollection<GameItem>(_items); }

        public bool ContainsItem(GameItem item)
        {
            return _items.Contains(item);
        }

        public void AddItem(GameItem item)
        {
            _items.Add(item);

            var oldParent = item.Parent;
            if (oldParent != null)
                oldParent.RemoveItem(item);

            item.Parent = this;
            if (_parent is Player)
            {
                PendingUpdates.Instance.Add(PendingUpdateId.Inventory);
            }
        }

        public void RemoveItem(GameItem item)
        {
            if (_parent is Player)
            {
                PendingUpdates.Instance.Add(PendingUpdateId.Inventory);
            }

            _items.Remove(item);
            item.Parent = null;
        }
    }
}
