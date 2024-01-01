using System.Collections.Generic;
using UnityEngine;


namespace Ventura.GameLogic
{
    public class MapStack
    {
        public record MapStackItem
        {
            public readonly string mapName;
            public Vector2Int? pos;

            public MapStackItem(string mapName, Vector2Int? pos)
            {
                this.mapName = mapName;
                this.pos = pos;
            }
        }

        private Stack<MapStackItem> _data = new();
        public int Count { get => _data.Count; }

        public string CurrMapName { get => _data.Peek().mapName; }

        public List<string> GetStackMapNames()
        {
            var res = new List<string>();

            var en = _data.GetEnumerator();
            while (en.MoveNext())
            {
                res.Add(en.Current.mapName);
            }

            return res;
        }


        public void PushMap(string newMapName, Vector2Int? oldPos)
        {
            if (_data.Count > 0)
                _data.Peek().pos = oldPos;

            this._data.Push(new MapStackItem(newMapName, null));

            PendingUpdates.Instance.Add(PendingUpdateId.MapTerrain);
        }


        public Vector2Int PopMap()
        {
            _data.Pop();
            if (_data.Count == 0)
            {
                throw new GameException(
                    "Inconsistency in CurrMapStack",
                    "this.currMapis is valid",
                    "this.currMapis is null");
            }

            var currPos = _data.Peek().pos;
            if (currPos == null)
            {
                throw new GameException(
                    "Inconsistency in CurrMapStack",
                    "this.currMap.pos is valid",
                    "this.currMap.pos is null");
            }

            PendingUpdates.Instance.Add(PendingUpdateId.MapTerrain);

            return (Vector2Int)currPos;
        }
    }
}


