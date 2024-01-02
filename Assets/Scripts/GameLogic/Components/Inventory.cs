
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Ventura.Util;

namespace Ventura.GameLogic.Components
{
    [Serializable]
    public class Inventory : Container, ISerializationCallbackReceiver
    {
        protected Actor _parent;
        public Actor Parent { get => _parent; set => _parent = value; }

        [SerializeReference]
        private List<GameItem> _items = new();

        [SerializeField]
        private int _maxSize;

        public bool IsFull { get => (_items.Count == _maxSize); }

        public Inventory(Actor parent, int maxSize)
        {
            _parent = parent;
            _maxSize = maxSize;
        }

        // -------- Custom Serialization -------------------
        public virtual void OnBeforeSerialize()
        {

        }

        public virtual void OnAfterDeserialize()
        {
            foreach (var gameItem in _items)
            {
                gameItem.Parent = this;
            }
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
                Orchestrator.Instance.PendingUpdates.Add(PendingUpdateId.Inventory);
        }

        public void RemoveItem(GameItem item)
        {
            if (_parent is Player)
                Orchestrator.Instance.PendingUpdates.Add(PendingUpdateId.Inventory);

            _items.Remove(item);
            item.Parent = null;
        }

        public void Dump()
        {
            DebugUtils.Log($"maxSize: {_maxSize}");
            foreach (var gameItem in _items)
                gameItem.Dump();
        }

    }
}
