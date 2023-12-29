

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ventura.GameLogic
{
    public interface Container
    {
        public ReadOnlyCollection<GameItem> Items { get; }

        public bool ContainsItem(GameItem item);
        public void AddItem(GameItem item);
        public void RemoveItem(GameItem item);

        public bool IsFull { get; }
    }

    public class Inventory : Container
    {
        private List<GameItem> _items = new();

        private int _maxSize;

        public bool IsFull { get => (_items.Count == _maxSize); }

        public Inventory(int maxSize)
        {
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
        }

        public void RemoveItem(GameItem item)
        {
            _items.Remove(item);

            item.Parent = null;
        }
    }
}
