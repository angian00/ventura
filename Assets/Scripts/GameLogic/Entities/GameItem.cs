using System;
using UnityEngine;
using Ventura.GameLogic.Components;

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


        public GameItem(GameItemTemplate template) : base(template.Name, false)
        {
            if (template.BaseColor != null)
            {
                Color c;
                if (ColorUtility.TryParseHtmlString(template.BaseColor, out c))
                    this._color = c;
            }

            this._spriteId = template.SpriteId ?? "unknownItem";
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

        public void TransferTo(Container targetContainer)
        {
            if (_parent != null)
                _parent.RemoveItem(this);

            targetContainer.AddItem(this);
        }
    }
}
