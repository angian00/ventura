using System;
using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.Util;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Actor : Entity, ISerializationCallbackReceiver
    {
        [NonSerialized]
        protected AI _ai = null;

        [SerializeReference]
        protected Skills _skills = null;
        public Skills Skills { get => _skills; }

        [SerializeReference]
        protected Inventory _inventory = null;
        public Inventory Inventory { get => _inventory; }
        //private Equipment? _equipment;


        public Actor(string name) : base(name, true) { }

        // -------- Custom Serialization -------------------
        public virtual void OnBeforeSerialize()
        {
            DebugUtils.Log($"Actor {_name}.OnBeforeSerialize()");
        }

        public virtual void OnAfterDeserialize()
        {
            DebugUtils.Log($"Actor {_name}.OnAfterDeserialize()");

            //if (this is Player)
            //    _ai = new PlayerAI(this);

            if (_ai != null)
                _ai.Parent = this;

            if (_inventory != null)
                _inventory.Parent = this;

            if (_skills != null)
                _skills.Parent = this;
        }

        // -------------------------------------------------

        public override void MoveTo(int x, int y)
        {
            base.MoveTo(x, y);
        }

        public ActionData ChooseAction()
        {
            return _ai?.ChooseAction();
        }

        //public void Die()
        //{
        //}

        public override void Dump()
        {
            DebugUtils.Log($"Actor {_name}; x={_x}, y={_y}");

            if (_inventory != null)
            {
                //DebugUtils.Log($"has inventory:");
                //_inventory.Dump();
            }

            if (_skills != null)
            {
                //DebugUtils.Log($"has skills:");
                //_skills.Dump();
            }
        }
    }

}
