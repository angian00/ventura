
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Ventura.Unity.Events;
using Ventura.Util;
using Random = UnityEngine.Random;


namespace Ventura.GameLogic
{
    [Serializable]
    public class GameMap : GameLogicObject, Container, ISerializationCallbackReceiver
    {
        const bool MAP_DEBUGGING = false;
        //const bool MAP_DEBUGGING = true;

        const float VISIBILITY_RADIUS = 4.0f;

        [SerializeField]
        private string _name;
        public string Name { get => _name; }

        [SerializeField]
        private string _label;
        public string Label { get => _label; }

        [SerializeField]
        private int _width;
        public int Width { get => _width; }

        [SerializeField]
        private int _height;
        public int Height { get => _height; }

        [SerializeField]
        private Vector2Int _startingPos;
        public Vector2Int StartingPos { get => _startingPos; set => _startingPos = value; }


        private TerrainType[,] _terrain;
        public TerrainType[,] Terrain { get => _terrain; }

        private bool[,] _visible;
        public bool[,] Visible { get => _visible; }

        private bool[,] _explored;
        public bool[,] Explored { get => _explored; }

        private HashSet<Entity> _entities = new();
        public HashSet<Entity> Entities { get => _entities; }


        public GameMap(string name, string label, int width, int height, TerrainType[,] terrain)
        {
            this._name = name;
            this._label = label;
            this._width = width;
            this._height = height;
            this._terrain = terrain;

            _visible = new bool[width, height];
            _explored = new bool[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _visible[x, y] = false;
                    _explored[x, y] = false;

                    if (MAP_DEBUGGING)
                        _explored[x, y] = true;
                }
            }
        }

        public GameMap(string name, string label, int width, int height) : this(name, label, width, height, makeDefaultTerrain(width, height))
        { }


        /// -------- Custom Serialization -------------------

        [SerializeField]
        private string[] _auxTerrain;

        [SerializeField]
        private bool[] _auxExplored;

        [SerializeField]
        private bool[] _auxVisible;

        [SerializeReference]
        private List<Entity> _auxEntities;


        public void OnBeforeSerialize()
        {
            Debug.Log($"GameMap.OnBeforeSerialize()");

            _auxTerrain = new string[_width * _height];

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _auxTerrain[x * _height + y] = _terrain[x, y].Name;
                }
            }

            _auxVisible = DataUtils.Flatten(_visible);
            _auxExplored = DataUtils.Flatten(_explored);
            _auxEntities = new List<Entity>(_entities);
        }

        public void OnAfterDeserialize()
        {
            Debug.Log($"GameMap.OnAfterDeserialize()");

            _terrain = new TerrainType[_width, _height];
            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                    _terrain[x, y] = TerrainType.FromName(_auxTerrain[x * _height + y]);


            _visible = DataUtils.Unflatten(_auxVisible, _width, _height);
            _explored = DataUtils.Unflatten(_auxExplored, _width, _height);
            _entities = new HashSet<Entity>(_auxEntities);

            foreach (var e in _entities)
            {
                if (e is GameItem)
                    ((GameItem)e).Parent = this;
            }    
        }

        /// ------------------------------------------------------
        
        
        public void DumpEntities()
        {
            foreach (var e in _entities)
            {
                DebugUtils.Log($"Found entity {e.Name} of type {e.GetType()}");
                e.Dump();
            }
        }


        private static TerrainType[,] makeDefaultTerrain(int w, int h)
        {
            var terrain = new TerrainType[w, h];

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    terrain[x, y] = TerrainType.Plains1;
                }
            }

            return terrain;
        }

        public bool IsInBounds(int x, int y)
        {
            return (0 <= x && x < _width && 0 <= y && y < _height);
        }


        public bool IsWalkable(int x, int y)
        {
            return (IsInBounds(x, y) && Terrain[x, y].Walkable && GetBlockingEntityAt(x, y) == null);
        }

        public T? GetAnyEntityAt<T>(int x, int y) where T: Entity
        {
            foreach (var e in _entities)
            {
                if ((e.x == x) && (e.y == y) && (e is Site))
                    return e as T;
            }

            return null;
        }

        public T? GetAnyEntityAt<T>(Vector2Int pos) where T: Entity
        {
            return GetAnyEntityAt<T>(pos.x, pos.y);
        }


        public ReadOnlyCollection<T> GetAllEntitiesAt<T>(int x, int y) where T: Entity
        {
            var result = new List<T>();

            foreach (var e in _entities)
            {
                if (e.x == x && e.y == y && e is T)
                    result.Add((T)e);

            }

            return new ReadOnlyCollection<T>(result);
        }


        public T? GetAnyEntity<T>() where T : Entity
        {
            foreach (var e in _entities)
            {
                if (e is T)
                    return (T)e;
            }

            return null;
        }

        public ReadOnlyCollection<T> GetAllEntities<T>() where T : Entity
        {
            var result = new List<T>();

            foreach (var e in _entities)
            {
                if (e is T)
                    result.Add((T)e);
            }

            return new ReadOnlyCollection<T>(result);
        }


        public Entity? GetBlockingEntityAt(int x, int y)
        {
            foreach (var e in _entities)
            {
                if (e.x == x && e.y == y & e.IsBlocking)
                    return e;
            }
            return null;
        }


        public Vector2Int? GetRandomWalkablePos()
        {
            int x, y;

            while (true)
            {
                x = Random.Range(0, _width);
                y = Random.Range(0, _height);

                if (_terrain[x, y].Walkable && (GetBlockingEntityAt(x, y) == null))
                    return new Vector2Int(x, y);
            }
        }


        public void UpdateExploration(int targetX, int targetY)
        {
            var r = VISIBILITY_RADIUS;

            //GameDebugging.Log("UpdateExploration");
            //fov.compute(this.player.x, this.player.y, lightRadius, this.setFov.bind(this))

            //reset visible
            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                    _visible[x, y] = false;


            //FUTURE: use Unity line-of-sight algorithm
            var startX = (int)Math.Max(targetX - r, 0);
            var endX = (int)Math.Min(targetX + r, _width - 1);
            var startY = (int)Math.Max(targetY - r, 0);
            var endY = (int)Math.Min(targetY + r, _height - 1);

            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    _visible[x, y] = true;
                    _explored[x, y] = true;
                }
            }

            EventManager.MapUpdateEvent.Invoke(this);
        }


        public ReadOnlyCollection<GameItem> Items
        {
            get
            {
                List<GameItem> res = new();
                foreach (var e in _entities)
                {
                    if (e is GameItem)
                        res.Add((GameItem)e);
                }

                return new ReadOnlyCollection<GameItem>(res);
            }
        }


        public bool IsFull { get => false; }

        public bool ContainsItem(GameItem item)
        {
            return _entities.Contains(item);
        }

        public void AddItem(GameItem item)
        {
            _entities.Add(item);
        }

        public void RemoveItem(GameItem item)
        {
            _entities.Remove(item);
        }
    }
}
