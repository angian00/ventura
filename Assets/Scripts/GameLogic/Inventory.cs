

using System.Collections.Generic;

namespace Ventura.GameLogic
{
    public interface Container
    {
        //public List<GameItem> Items { get; }
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

        //public List<GameItem> Items { get => _items; }

        public bool ContainsItem(GameItem item)
        {
            return _items.Contains(item);
        }

        public void AddItem(GameItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(GameItem item)
        {
            _items.Remove(item);
        }

    }

}
