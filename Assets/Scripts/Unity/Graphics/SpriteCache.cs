using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Unity.Graphics
{

    public class SpriteCache
    {
        private static SpriteCache _instance = new SpriteCache();
        public static SpriteCache Instance { get { return _instance; } }

        private Dictionary<string, Sprite> _sprites = new();

        //public Sprite GetSprite(Entity e)
        //{
        //    Sprite sprite;
        //    if (_sprites.ContainsKey(e))
        //        sprite = _sprites[e];
        //    else
        //    {
        //        sprite = SpriteLoader.Load(e);
        //        _sprites.Add(e, sprite);
        //    }

        //    return sprite;
        //}

        public Sprite GetSprite(string spriteId)
        {
            Sprite sprite;
            if (_sprites.ContainsKey(spriteId))
                sprite = _sprites[spriteId];
            else
            {
                sprite = LoadSprite(spriteId);
                _sprites.Add(spriteId, sprite);
            }

            return sprite;
        }


        private static Sprite LoadSprite(string spriteId)
        {
            var spriteName = GraphicsConfigOld.SpriteFiles[spriteId];
            var sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
            if (sprite == null)
                throw new GameException($"!! Sprite not found: {spriteName}");

            return sprite;
        }

    }
}