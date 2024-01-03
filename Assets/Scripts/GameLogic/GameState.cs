
using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.Generators;
using Ventura.Unity.Events;
using Ventura.Util;


namespace Ventura.GameLogic
{
    [Serializable]
    public class GameState: ISerializationCallbackReceiver
    {
        private Dictionary<string, GameMap> _allMaps;

        [SerializeField]
        private MapStack _currMapStack;
        public MapStack CurrMapStack { get => _currMapStack; }

        private GameMap _currMap;
        public GameMap CurrMap { get => _currMap; set => _currMap = value; }

        private Player _player;
        public Player Player { get => _player; set => _player = value;  }



        public GameState()
        {
            Clear();
        }

        /// -------- Custom Serialization -------------------

        [SerializeField]
        private List<string> __auxAllMapsKeys;

        [SerializeReference]
        private List<GameMap> __auxAllMapsValues;


        public void OnBeforeSerialize()
        {
            Debug.Log($"GameState.OnBeforeSerialize");

            __auxAllMapsKeys = new ();
            __auxAllMapsValues = new ();

            foreach (var mapName in _allMaps.Keys)
            {
                __auxAllMapsKeys.Add(mapName);
                __auxAllMapsValues.Add(_allMaps[mapName]);
            }

        }

        public void OnAfterDeserialize()
        {
            Debug.Log($"GameState.OnAfterDeserialize");
            Debug.Assert(__auxAllMapsKeys.Count == __auxAllMapsValues.Count);

            _allMaps = new();
            for (var i = 0; i < __auxAllMapsKeys.Count; i++)
                _allMaps.Add(__auxAllMapsKeys[i], __auxAllMapsValues[i]);

            _currMap = _allMaps[_currMapStack.CurrMapName];
            _player = _currMap.GetAnyEntity<Player>();
        }

        /// ------------------------------------------------------


        public void Clear()
        {
            _allMaps = new();
            _currMapStack = new();
            _currMap = null;
            _player = null;
        }

        public void NewGame()
        {
            DebugUtils.Log("GameState.NewGame()");

            const int WORLD_MAP_WIDTH = 80;
            const int WORLD_MAP_HEIGHT = 65;
            var startMap = MapGenerator.GenerateWildernessMap(WORLD_MAP_WIDTH, WORLD_MAP_HEIGHT);
            _currMap = startMap;

            _allMaps[startMap.Name] = startMap;
            _currMapStack = new MapStack();
            _currMapStack.PushMap(startMap.Name);
            EventManager.LocationChangeEvent.Invoke(_currMap, _currMapStack.StackMapNames);

            _player = PlayerGenerator.GeneratePlayerWithBooks();
            _currMap.Entities.Add(_player);
            MoveActorTo(_player, _currMap.StartingPos.x, _currMap.StartingPos.y);
        }



        public Vector2Int MoveActorTo(Actor a, int targetX, int targetY)
        {
            if (!_currMap.IsWalkable(targetX, targetY))
                throw new GameException($"Target tile {targetX}, {targetY} is not walkable");

            if (_currMap.IsWalkable(targetX, targetY))
            {
                a.MoveTo(targetX, targetY);

                if (a is Player)
                {
                    DebugUtils.Log($"MoveActorTo {targetX}, {targetY}");
                    _currMap.UpdateExploration(targetX, targetY);
                }
            }

            return new Vector2Int(a.x, a.y);
        }


        public void EnterMap(string mapName)
        {
            //Orchestrator.Instance.DeactivateAllActors(); //FIXME

            _currMap.Entities.Remove(_player);

            GameMap newMap;
            if (_allMaps.ContainsKey(mapName))
            {
                newMap = _allMaps[mapName];
            }
            else
            {
                newMap = MapGenerator.GenerateWildernessMap(10, 10, mapName, false);
                _allMaps[mapName] = newMap;
            }

            _currMapStack.PushMap(mapName, new Vector2Int(_player.x, _player.y));
            _currMap = newMap;

            EventManager.LocationChangeEvent.Invoke(_currMap, _currMapStack.StackMapNames);

            _currMap.Entities.Add(_player);

            var startPos = _currMap.StartingPos;
            DebugUtils.Log($"EnterMap; startPos={startPos}");
            if (startPos == null)
                startPos = (Vector2Int)_currMap.GetRandomWalkablePos();

            MoveActorTo(_player, startPos.x, startPos.y);

            //Orchestrator.Instance.ActivateActors(_currMap.GetAllEntities<Actor>()); //FIXME
        }

        public void ExitMap()
        {
            //Orchestrator.Instance.DeactivateAllActors(); //FIXME

            _currMap.Entities.Remove(_player);

            var previousMapPos = _currMapStack.PopMap();
            _currMap = _allMaps[_currMapStack.CurrMapName];
            Debug.Assert(_currMap != null);
            EventManager.LocationChangeEvent.Invoke(_currMap, _currMapStack.StackMapNames);

            _currMap.Entities.Add(_player);

            MoveActorTo(_player, previousMapPos.x, previousMapPos.y);

            //Orchestrator.Instance.ActivateActors(_currMap.GetAllEntities<Actor>()); //FIXME
        }
    }
}
