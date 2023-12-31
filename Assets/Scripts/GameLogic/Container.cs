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
}
