using System;
using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;

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
        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            //DebugUtils.Log($"Actor {_name}.OnBeforeSerialize()");
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            //DebugUtils.Log($"Actor {_name}.OnAfterDeserialize()");

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

    }

}
