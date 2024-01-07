using System;
using UnityEngine;
using Ventura.Unity.Events;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public abstract class Entity : GameLogicObject
    {
        [SerializeField]
        protected Guid _id;
        public Guid Id { get => _id; }


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
            _id = Guid.NewGuid();
        }

        protected Entity(string name, bool isBlocking = false) : this()
        {
            _name = name;
            _isBlocking = isBlocking;

            _x = 0;
            _y = 0;
        }

        public virtual void MoveTo(int x, int y)
        {
            _x = x;
            _y = y;

            EventManager.Publish(new EntityUpdate(EntityUpdate.Type.Changed, this));
        }

        public abstract void Dump();
    }

}
