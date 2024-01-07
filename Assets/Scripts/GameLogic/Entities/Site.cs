using System;
using UnityEngine;
using Ventura.Util;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Site : Entity
    {
        [SerializeField]
        private string _mapName;
        public string MapName { get => _mapName; }

        public Site(string name, string mapName) : base(name, false)
        {
            _mapName = mapName;
        }

        public override void Dump()
        {
            DebugUtils.Log($"Site {_name}");
        }

    }
}
