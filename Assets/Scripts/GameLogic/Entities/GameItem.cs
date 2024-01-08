using System;
using UnityEngine;
using Ventura.GameLogic.Components;
using Ventura.Util;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class GameItem : Entity, ISerializationCallbackReceiver
    {
        protected Container _parent;
        public Container Parent { get => _parent; set => _parent = value; }

        [SerializeReference]
        protected Consumable _consumable;
        public Consumable Consumable { get => _consumable; }
        //protected Equippable? _equippable;
        //protected Combinable? _combinable;

        public virtual string Label { get => _name; }

        public GameItem(string name) : base(name, false) { }


        // -------- Custom Serialization -------------------
        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            if (_consumable != null)
                _consumable.Parent = this;
        }

        // -------------------------------------------------
        public override void Dump()
        {
            DebugUtils.Log($"GameItem {_name}");
            if (_consumable != null)
                DebugUtils.Log($"has consumable");
        }

        public void TransferTo(Container targetContainer)
        {
            if (_parent != null)
                _parent.RemoveItem(this);

            targetContainer.AddItem(this);
        }
    }
}
