
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ventura.Util;

namespace Ventura.GameLogic
{

    public enum PendingUpdateId
    {
        MapTerrain,
        MapPlayerPos,
        MapItems,
        Inventory,
    }


    public class PendingUpdates
    {
        private static PendingUpdates _instance = new PendingUpdates();
        public static PendingUpdates Instance { get => _instance; }

        private SortedSet<PendingUpdateId> _data = new();

        public ReadOnlyCollection<PendingUpdateId> GetAll()
        {
            return new ReadOnlyCollection<PendingUpdateId>(_data.ToList());
        }

        public void Add(PendingUpdateId updateId)
        {
            _data.Add(updateId);
        }

        public void AddAll()
        {
            foreach (var updateId in DataUtils.EnumValues<PendingUpdateId>())
                _data.Add(updateId);
        }

        public bool Contains(PendingUpdateId updateId)
        {
            return _data.Contains(updateId);
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}
