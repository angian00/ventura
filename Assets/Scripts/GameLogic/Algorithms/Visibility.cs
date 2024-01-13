
// Naive homecooked shadowcasting, based on Bresenham

namespace Ventura.GameLogic.Algorithms
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Visibility
    {
        private int _w;
        private int _h;
        private bool[,] _blockingTiles;
        private bool[,] _visible;



        public Visibility(bool[,] blockingTiles)
        {
            this._blockingTiles = blockingTiles;
            this._w = blockingTiles.GetLength(0);
            this._h = blockingTiles.GetLength(1);

            this._visible = new bool[_w, _h];
        }



        public bool[,] ComputeVisibility(Vector2Int startPos, float radius)
        {
            var tilesInRadius = computeTilesInRadius(startPos, radius);

            foreach (var tile in tilesInRadius)
            {
                var ray = Bresenham.ComputeLine(startPos, tile);
                foreach (var tileInRay in ray)
                {
                    _visible[tileInRay.x, tileInRay.y] = true;
                    if (_blockingTiles[tileInRay.x, tileInRay.y])
                        break;
                }
            }

            return _visible;
        }


        private List<Vector2Int> computeTilesInRadius(Vector2Int pos, float radius)
        {
            List<Vector2Int> res = new();

            var radius2 = radius * radius;
            var startX = pos.x;
            var startY = pos.y;

            for (var x = (int)Math.Floor(startX - radius); x <= (int)Math.Ceiling(startX + radius); x++)
            {
                for (var y = (int)Math.Floor(startY - radius); y <= (int)Math.Ceiling(startY + radius); y++)
                {
                    if (isInBounds(x, y) && ((x - startX) * (x - startX) + (y - startY) * (y - startY) <= radius2))
                        res.Add(new Vector2Int(x, y));
                }
            }

            return res;
        }

        private bool isInBounds(int x, int y)
        {
            return (x >= 0 && x < _w && y >= 0 && y < _h);
        }

    }

}