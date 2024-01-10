using System;
using UnityEngine;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Actor : Entity, ISerializationCallbackReceiver
    {
        protected int _currHP;
        public int CurrHP { get => _currHP; set => _currHP = value; }

        protected int _maxHP;
        public int MaxHP { get => _maxHP; }


        [SerializeReference]
        protected Skills _skills = null;
        public Skills Skills { get => _skills; }

        [SerializeReference]
        protected Inventory _inventory = null;
        public Inventory Inventory { get => _inventory; }
        //private Equipment? _equipment;

        public virtual CombatStats? CombatStats { get; }


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

            if (_inventory != null)
                _inventory.Parent = this;

            if (_skills != null)
                _skills.Parent = this;
        }

        // -------------------------------------------------

    }

}
