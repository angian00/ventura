﻿using System;
using UnityEngine;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Site : Entity
    {
        [SerializeField]
        private string _mapName;
        public string MapName { get => _mapName; }

        public override string SpriteId { get => "site"; } //FIXME: create SiteTemplate

        public Site(string name, string mapName) : base(name, false)
        {
            _mapName = mapName;
        }





        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
        }
    }
}
