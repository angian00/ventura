using System.Collections.Generic;
using UnityEngine;


namespace Ventura.GameLogic
{
    public class World
    {
        public record MapStackItem
        {
            public readonly GameMap map;
            public Vector2Int? pos;

            public MapStackItem(GameMap map, Vector2Int? pos)
            {
                this.map = map;
                this.pos = pos;
            }
        }

        private Stack<MapStackItem> _mapStack = new();
        public int MapStackSize { get => _mapStack.Count; }

        public List<string> GetStackMapNames()
        {
            var res = new List<string>();

            var en = _mapStack.GetEnumerator();
            while (en.MoveNext())
            {
                res.Add(en.Current.map.Name);
            }

            return res;
        }

        public GameMap CurrMap
        {
            get
            {
                if (_mapStack.Count == 0)
                    return null;

                return _mapStack.Peek().map;
            }
        }


        public void PushMap(GameMap newMap, Vector2Int? oldPos)
        {
            if (_mapStack.Count > 0)
                _mapStack.Peek().pos = oldPos;

            this._mapStack.Push(new MapStackItem(newMap, null));

            PendingUpdates.Instance.Add(PendingUpdateId.MapTerrain);
        }


        public Vector2Int PopMap()
        {
            _mapStack.Pop();
            if (CurrMap == null)
            {
                throw new GameException(
                    "Inconsistency in World",
                    "this.currMapis is valid",
                    "this.currMapis is null");
            }

            var currPos = _mapStack.Peek().pos;
            if (currPos == null)
            {
                throw new GameException(
                    "Inconsistency in World",
                    "this.currMap.pos is valid",
                    "this.currMap.pos is null");
            }

            PendingUpdates.Instance.Add(PendingUpdateId.MapTerrain);

            return (Vector2Int)currPos;
        }
    }
}


