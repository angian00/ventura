using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Loaders
{

    public class SpriteLoader
    {
        public static Sprite? Load(Entity e)
        {
            if (!(e is BookItem))
            {
                DebugUtils.Error($"Cannot LoadSprite for Entity {e}");
                return null;
            }

            return Resources.Load<Sprite>($"Sprites/book_00");
        }
    }
}