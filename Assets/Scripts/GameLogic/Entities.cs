using Ventura.Unity.Behaviours;
using Ventura.GameLogic.Components;
using Ventura.Util;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Ventura.GameLogic
{
    [Serializable]
    public abstract class Entity : GameLogicObject
    {
        [SerializeField]
        protected string _name;
        public string Name { get => _name; }

        [SerializeField] 
        protected int _x;
        public int x { get => _x; }

        [SerializeField] 
        protected int _y;
        public int y { get => _y; }

        [SerializeField]
        protected bool _isBlocking;
        public bool IsBlocking { get => _isBlocking; }


        protected Entity()
        {

        }

        protected Entity(string name, bool isBlocking = false)
        {
            this._name = name;
            //this.char = char
            //this.color = color
            this._isBlocking = isBlocking;

            this._x = 0;
            this._y = 0;
        }

        public virtual void MoveTo(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public abstract void Dump();
    }


    [Serializable]
    public class Actor : Entity
    {
        protected Orchestrator _orch;
        protected AI? _ai;

        [SerializeField]
        protected Skills? _skills;
        public Skills? Skills { get => _skills; }

        [SerializeField]
        protected Inventory? _inventory;
        public Inventory Inventory { get => _inventory; }
        //private Equipment? _equipment;


        public Actor(): base()
        {
            _orch = Orchestrator.Instance; //FUTURE: minize singletons
        }

        public Actor(Orchestrator orch, string name) : base(name, true)
        {
            this._orch = orch;
        }

        // -------- Custom Serialization -------------------
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (_ai != null)
            {
                _ai.Orchestrator = _orch;
                _ai.Parent = this;
            }

            if (_inventory != null)
                _inventory.Parent = this;
            
            if (_skills != null)
                _skills.Parent = this;
        }

        // -------------------------------------------------


        public void Act()
        {
            if (_ai != null)
            {
                var a = _ai.ChooseAction();
                if (a == null)
                    return;
                //DebugUtils.Log($"Performing ${a.GetType().Name}");

                var actionResult = a.Perform();
                if (actionResult.Reason != null)
                    StatusLineManager.Instance.Display(actionResult.Reason, 
                        actionResult.Success ? StatusSeverity.Normal : StatusSeverity.Warning);
            }
        }

        //public void Die()
        //{
        //}

        public override void Dump()
        {
            DebugUtils.Log($"Actor {_name}");
            //DebugUtils.Log($"_orch is {(_orch == null ? "" : "NOT")} null");
            if (_inventory != null)
            {
                DebugUtils.Log($"has inventory:");
                _inventory.Dump();
            }

            if (_skills != null)
            {
                DebugUtils.Log($"has skills:");
                _skills.Dump();
            }
        }
    }


    [Serializable]
    public class Player : Actor
    {
        public Player() : base() { }

        public Player(Orchestrator orch, string name) : base(orch, name)
        {
            _ai = new PlayerAI(_orch, this);
            _inventory = new Inventory(this, 999);
            _skills = new Skills(this);
        }

        public override void MoveTo(int x, int y)
        {
            base.MoveTo(x, y);
            PendingUpdates.Instance.Add(PendingUpdateId.MapPlayerPos);
        }
    }

    [Serializable]
    public class GameItem : Entity, ISerializationCallbackReceiver
    {
        protected Container? _parent;
        public Container Parent { get => _parent; set => _parent = value; }

        protected Consumable? _consumable;
        public Consumable Consumable { get => _consumable; }
        //protected Equippable? _equippable;
        //protected Combinable? _combinable;

        public virtual string Label { get => _name; }

        public GameItem(string name) : base(name, false) { }


        // -------- Custom Serialization -------------------
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
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
    }

    [Serializable]
    public class Site : Entity
    {
        [SerializeField]
        private string? _mapName;
        public string? MapName { get => _mapName; }

        public Site(string name, string mapName) : base(name, false)
        {
            this._mapName = mapName;
        }

        public override void Dump()
        {
            DebugUtils.Log($"Site {_name}");
        }

    }
}
