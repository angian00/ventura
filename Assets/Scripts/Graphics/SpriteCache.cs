using UnityEngine;
using System.Collections.Generic;
using Ventura.GameLogic;
using Ventura.Behaviours;
using Ventura.Loaders;

namespace Ventura.Graphics
{

    public class SpriteCache
    {
        private static SpriteCache _instance = new SpriteCache();
        public static SpriteCache Instance { get { return _instance; } }

        private Dictionary<Entity, Sprite> _sprites = new();
        
        public Sprite GetSprite(Entity e)
        {
            Sprite sprite;
            if (_sprites.ContainsKey(e))
                sprite = _sprites[e];
            else
            {
                sprite = SpriteLoader.Load(e);
                _sprites.Add(e, sprite);
            }

            return sprite;
        }
    }
}