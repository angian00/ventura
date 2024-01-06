﻿using System;
using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.Unity.Events;
using Ventura.Util;

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

        public Vector2Int pos { get => new Vector2Int(x, y); }


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
    public class Actor : Entity, ISerializationCallbackReceiver
    {
        [NonSerialized]
        protected AI? _ai = null;

        [SerializeReference]
        protected Skills? _skills = null;
        public Skills? Skills { get => _skills; }

        [SerializeReference]
        protected Inventory? _inventory = null;
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
            EventManager.GameStateUpdateEvent.Invoke(new ActorUpdateData(this));
        }

        public ActionData? ChooseAction()
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


    [Serializable]
    public class Player : Actor
    {
        public Player(string name) : base(name)
        {
            //_ai = new PlayerAI(this);
            _inventory = new Inventory(this, 999);
            _skills = new Skills(this);
        }
    }

    [Serializable]
    public class GameItem : Entity, ISerializationCallbackReceiver
    {
        protected Container? _parent;
        public Container Parent { get => _parent; set => _parent = value; }

        [SerializeReference]
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

        public void TransferTo(Container targetContainer)
        {
            if (_parent != null)
                _parent.RemoveItem(this);

            targetContainer.AddItem(this);
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
