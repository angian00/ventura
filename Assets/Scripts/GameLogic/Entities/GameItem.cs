using System;
using UnityEngine;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class GameItem : Entity, ISerializationCallbackReceiver
    {
        [SerializeReference]
        protected GameItemTemplate _template;

        protected Container _parent;
        public Container Parent { get => _parent; set => _parent = value; }

        [SerializeReference]
        protected Consumable _consumable;
        public Consumable Consumable { get => _consumable; }
        //protected Equippable? _equippable;
        //protected Combinable? _combinable;

        public override string Color { get => _template.BaseColor; }
        public override string SpriteId { get => _template.SpriteId; }


        public GameItem(GameItemTemplate template) : base(template.Name, false)
        {
            this._template = template;

            //FUTURE: set consumable, ...
        }

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


        //FIXME: remove
        public void TransferTo(Container targetContainer)
        {
            if (_parent != null)
                _parent.RemoveItem(this);

            targetContainer.AddItem(this);
        }
    }
}
