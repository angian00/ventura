using System;
using System.Collections.Generic;
using UnityEngine;


namespace Ventura.GameLogic
{
    [Serializable]
    public class MapStack: ISerializationCallbackReceiver
    {
        [Serializable]
        private record MapStackItem
        {
            public static Vector2Int DEFAULT_POS = new Vector2Int(-1, -1); //can't use null since Vector2Int is a type value

            [SerializeField]
            public string mapName;

            [SerializeReference]
            public Vector2Int lastPos;

            public MapStackItem(string mapName, Vector2Int lastPos)
            {
                this.mapName = mapName;
                this.lastPos = lastPos;
            }

            public MapStackItem(string mapName)
            {
                this.mapName = mapName;
                this.lastPos = new Vector2Int(-1, -1);
            }
        }

        private Stack<MapStackItem> _data = new();
        public int Count { get => _data.Count; }

        public string CurrMapName { get => _data.Peek().mapName; }

        /**
         * From widest scale to finest scale
         */
        public List<string> StackMapNames
        {
            get {
                var res = new List<string>();

                var en = _data.GetEnumerator();
                while (en.MoveNext())
                {
                    res.Add(en.Current.mapName);
                }

                res.Reverse();
                return res;
            }
        }

        // -------- Custom Serialization -------------------

        [SerializeField]
        private List<MapStackItem> __auxData;


        public void OnBeforeSerialize()
        {
            __auxData = new List<MapStackItem>(_data);
            __auxData.Reverse();
        }

        public void OnAfterDeserialize()
        {
            _data = new Stack<MapStackItem>(__auxData);
        }

        // -------------------------------------------------

        public void PushMap(string newMapName)
        {
            PushMap(newMapName, MapStackItem.DEFAULT_POS);
        }


        public void PushMap(string newMapName, Vector2Int currPos)
        {
            if (_data.Count > 0)
                _data.Peek().lastPos = currPos;

            this._data.Push(new MapStackItem(newMapName));

            Orchestrator.Instance.PendingUpdates.Add(PendingUpdateId.MapTerrain); //CHECK: where to put pendingupdates writes
        }


        public Vector2Int PopMap()
        {
            _data.Pop();
            if (_data.Count == 0)
            {
                throw new GameException(
                    "Inconsistency in CurrMapStack",
                    "stack is is valid",
                    "stack is empty");
            }

            var currPos = _data.Peek().lastPos;
            if (currPos == MapStackItem.DEFAULT_POS)
            {
                throw new GameException(
                    "Inconsistency in CurrMapStack",
                    "currPos is valid",
                    "currPos is uninitialized");
            }

            Orchestrator.Instance.PendingUpdates.Add(PendingUpdateId.MapTerrain); //CHECK: where to put pendingupdates writes

            return currPos;
        }
    }
}


