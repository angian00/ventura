
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Ventura.GameLogic.Entities;
using Ventura.Unity.Events;
using Ventura.Util;


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


        private TerrainDef[,] _terrain;
        public TerrainDef[,] Terrain { get => _terrain; }

        private bool[,] _visible;
        public bool[,] Visible { get => _visible; }

        private bool[,] _explored;
        public bool[,] Explored { get => _explored; }

        private HashSet<Entity> _entities = new();
        //only exposed through specific methods
        //public HashSet<Entity> Entities { get => _entities; }


        public GameMap(string name, string label, int width, int height)
        {
            this._name = name;
            this._label = label;
            this._width = width;
            this._height = height;

            _terrain = new TerrainDef[width, height];
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


        /// -------- Custom Serialization -------------------

        [SerializeField]
        private TerrainDef.TerrainType[] _auxTerrain;

        [SerializeField]
        private bool[] _auxExplored;

        [SerializeField]
        private bool[] _auxVisible;

        [SerializeReference]
        private List<Entity> _auxEntities;


        public void OnBeforeSerialize()
        {
            //Debug.Log($"GameMap.OnBeforeSerialize()");

            _auxTerrain = new TerrainDef.TerrainType[_width * _height];

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _auxTerrain[x * _height + y] = _terrain[x, y].Type;
                }
            }

            _auxVisible = DataUtils.Flatten(_visible);
            _auxExplored = DataUtils.Flatten(_explored);
            _auxEntities = new List<Entity>(_entities);
        }

        public void OnAfterDeserialize()
        {
            //Debug.Log($"GameMap.OnAfterDeserialize()");

            _terrain = new TerrainDef[_width, _height];
            for (var x = 0; x < _width; x++)
                for (var y = 0; y < _height; y++)
                    _terrain[x, y] = TerrainDef.FromType(_auxTerrain[x * _height + y]);


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

        public bool IsInBounds(int x, int y)
        {
            return (0 <= x && x < _width && 0 <= y && y < _height);
        }


        public bool IsWalkable(int x, int y)
        {
            return (IsInBounds(x, y) && Terrain[x, y].Walkable && GetBlockingEntityAt(x, y) == null);
        }

        public bool IsEmpty(int x, int y)
        {
            return (IsInBounds(x, y) && Terrain[x, y].Walkable && GetAnyEntityAt<Entity>(x, y) == null);
        }


        public void AddEntity(Entity entity)
        {
            AddEntity(entity, _startingPos);
        }

        public void AddEntity(Entity entity, Vector2Int pos)
        {
            _entities.Add(entity);
            if (entity is GameItem || entity.GetType().IsSubclassOf(typeof(GameItem)))
                ((GameItem)entity).Parent = this;

            EventManager.Publish(new EntityUpdate(EntityUpdate.Type.Added, entity));
            entity.MoveTo(pos.x, pos.y);
        }


        public void RemoveEntity(Entity entity)
        {
            if (entity is GameItem || entity.GetType().IsSubclassOf(typeof(GameItem)))
                ((GameItem)entity).Parent = null;

            _entities.Remove(entity);
            EventManager.Publish(new EntityUpdate(EntityUpdate.Type.Removed, entity));
        }

        public bool ContainsEntity(Entity entity)
        {
            return _entities.Contains(entity);
        }


        public T? GetAnyEntityAt<T>(int x, int y) where T : Entity
        {
            foreach (var e in _entities)
            {
                if ((e.x == x) && (e.y == y) && (e is T || e.GetType().IsSubclassOf(typeof(T))))
                    return e as T;
            }

            return null;
        }

        public T? GetAnyEntityAt<T>(Vector2Int pos) where T : Entity
        {
            return GetAnyEntityAt<T>(pos.x, pos.y);
        }


        public ReadOnlyCollection<T> GetAllEntitiesAt<T>(int x, int y) where T : Entity
        {
            var result = new List<T>();

            foreach (var e in _entities)
            {
                if (e.x == x && e.y == y && (e is T || e.GetType().IsSubclassOf(typeof(T))))
                    result.Add((T)e);

            }

            return new ReadOnlyCollection<T>(result);
        }


        public T? GetAnyEntity<T>() where T : Entity
        {
            foreach (var e in _entities)
            {
                if (e is T || e.GetType().IsSubclassOf(typeof(T)))
                    return (T)e;
            }

            return null;
        }

        public ReadOnlyCollection<T> GetAllEntities<T>() where T : Entity
        {
            var result = new List<T>();

            foreach (var e in _entities)
            {
                if (e is T || e.GetType().IsSubclassOf(typeof(T)))
                    result.Add((T)e);
            }

            return new ReadOnlyCollection<T>(result);
        }


        public ReadOnlyCollection<T> GetVisibleEntities<T>() where T : Entity
        {
            var result = new List<T>();

            foreach (var e in _entities)
            {
                if (_visible[e.x, e.y] && (e is T || e.GetType().IsSubclassOf(typeof(T))))
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


        public bool[,] GetBlockedTiles()
        {
            var blocked = new bool[_width, _height];
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    blocked[x, y] = (!_terrain[x, y].Walkable);
                }
            }

            foreach (var e in _entities)
            {
                if (e is Actor)
                    blocked[e.x, e.y] = true;
            }


            return blocked;
        }

        public Vector2Int MoveActorTo(Actor a, int targetX, int targetY)
        {
            if (GetAnyEntityAt<Actor>(targetX, targetY) != a && !IsWalkable(targetX, targetY))
                throw new GameException($"Target tile {targetX}, {targetY} is not walkable");

            if (a.x != targetX || a.y != targetY)
                a.MoveTo(targetX, targetY);

            if (a is Player)
                updateExploration(targetX, targetY);


            return new Vector2Int(a.x, a.y);
        }


        private void updateExploration(int targetX, int targetY)
        {
            var r = VISIBILITY_RADIUS;

            //GameDebugging.Log("updateExploration");
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

            EventManager.Publish(new GameStateUpdate(null, this, GameStateUpdate.UpdatedFields.Visibility));
        }


        // -------------------------- Container methods --------------------------

        public ReadOnlyCollection<GameItem> Items
        {
            get => GetAllEntities<GameItem>();
        }


        public bool IsFull { get => false; }

        public bool ContainsItem(GameItem item)
        {
            return ContainsEntity(item);
        }

        public void AddItem(GameItem item)
        {
            AddEntity(item);
        }

        public void RemoveItem(GameItem item)
        {
            RemoveEntity(item);
        }
    }
}
