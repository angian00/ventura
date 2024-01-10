using System;
using UnityEngine;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public abstract class EntityTemplate
    {
        [SerializeField]
        protected string _name;
        public string Name { get => _name; }

        [SerializeField]
        protected string _spriteId;
        public string SpriteId { get => _spriteId; }

        [SerializeField]
        protected string _baseColor;
        public string BaseColor { get => _baseColor; }

    }

}

