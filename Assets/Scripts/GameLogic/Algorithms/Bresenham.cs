
// Algorithm adapted from Wikipedia
// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#:~:text=Bresenham's%20line%20algorithm%20is%20a,straight%20line%20between%20two%20points

namespace Ventura.GameLogic.Algorithms
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Ventura.Util;

    public class Bresenham
    {
        public static List<Vector2Int> ComputeLine(Vector2Int startPos, Vector2Int endPos)
        {
            DebugUtils.Log($"ComputeLine; startPos: {startPos}, endPos: {endPos}");
            List<Vector2Int> line;
            bool flipped = false;

            if (Math.Abs(endPos.y - startPos.y) < Math.Abs(endPos.x - startPos.x))
            {
                if (startPos.x > endPos.x)
                {
                    flipped = true;
                    line = ComputeLineLow(endPos, startPos);
                }
                else
                    line = ComputeLineLow(startPos, endPos);
            }
            else
            {
                if (startPos.y > endPos.y)
                {
                    flipped = true;
                    line = ComputeLineHigh(endPos, startPos);
                }
                else
                    line = ComputeLineHigh(startPos, endPos);
            }

            if (flipped)
                line.Reverse();

            return line;
        }

        private static List<Vector2Int> ComputeLineHigh(Vector2Int startPos, Vector2Int endPos)
        {
            DebugUtils.Log("ComputeLineHigh");
            List<Vector2Int> res = new();

            var dx = endPos.x - startPos.x;
            var dy = endPos.y - startPos.y;

            var xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            var D = 2 * dx - dy;
            var x = startPos.x;


            for (var y = startPos.y; y <= endPos.y; y++)
            {
                res.Add(new Vector2Int(x, y));

                if (D > 0)
                {
                    x = x + xi;
                    D = D + 2 * (dx - dy);
                }
                else
                {
                    D = D + 2 * dx;
                }
            }

            return res;
        }


        private static List<Vector2Int> ComputeLineLow(Vector2Int startPos, Vector2Int endPos)
        {
            DebugUtils.Log("ComputeLineLow");
            List<Vector2Int> res = new();

            var dx = endPos.x - startPos.x;
            var dy = endPos.y - startPos.y;

            var yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            var D = 2 * dy - dx;
            var y = startPos.y;


            for (var x = startPos.x; x <= endPos.x; x++)
            {
                res.Add(new Vector2Int(x, y));

                if (D > 0)
                {
                    y = y + yi;
                    D = D + 2 * (dy - dx);
                }
                else
                {
                    D = D + 2 * dy;
                }
            }

            return res;
        }
    }
}