
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


#nullable enable
namespace Ventura.GameLogic
{
    public class GameMap: GameLogicObject
    {
        const bool MAP_DEBUGGING = false;
        //const bool MAP_DEBUGGING = true;

        private string _name;
        public string Name { get { return _name; } }

        private string _label;
        public string Label { get { return _label; } }

        private int _width;
        public int Width { get { return _width; } }

        private int _height;
        public int Height { get { return _height; } }

        private TerrainType[,] _terrain;
        public TerrainType[,] Terrain { get { return _terrain; } }

        private bool[,] _visible;
        public bool[,] Visible { get { return _visible; } }

        private bool[,] _explored;
        public bool[,] Explored { get { return _explored; } }

        private Vector2Int _startingPos;
        public Vector2Int StartingPos { get { return _startingPos; } set { _startingPos = value; } }

        private HashSet<Entity> _entities = new();
        public HashSet<Entity> Entities { get { return _entities; } }


        //public Orchestrator Orchestrator
        //{
        //    set
        //    {
        //        foreach (var e in _entities)
        //        {
        //            if (e is Actor)
        //            {
        //                ((Actor)e).Orchestrator = value;
        //            }
        //        }
        //    }
        //}

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

        public GameMap(string name, string label, int width, int height): this(name, label, width, height, makeDefaultTerrain(width, height))
        { }


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

        public bool IsInBounds(int x, int y) {
            return (0 <= x && x < _width && 0 <= y && y < _height);
        }

        public Site? GetSiteAt(int x, int y)
        {
            foreach (var e in _entities)
            {
                if ((e.X == x) && (e.Y == y) && (e is Site))
                    return e as Site;
            }

            return null;
        }
        
        public Site? GetSiteAt(Vector2Int pos)
        {
            return GetSiteAt(pos.x, pos.y);
        }


        public Actor? GetActorAt(int x, int y)
        {
            foreach (var e in _entities)
            {
                if ((e.X == x) && (e.Y == y) && (e is Actor))
                    return e as Actor;
            }

            return null;
        }

        public Actor? GetActorAt(Vector2Int pos)
        {
            return GetActorAt(pos.x, pos.y);
        }


        public List<Entity> GetEntitiesAt(int x, int y)
        {
            var result = new List<Entity>();

            foreach (var e in _entities)
            {
                if (e.X == x && e.Y == y)
                    result.Add(e);

            }

            return result;
        }

        public Entity? GetBlockingEntityAt(int x, int y)
        {
            foreach (var e in _entities)
            {
                if (e.X == x && e.Y == y & e.IsBlocking)
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

            return null;
        }


        public void UpdateExploration(int targetX, int targetY, float r)
        {
            //Messages.Log("UpdateExploration");

            //reset visible
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _visible[x, y] = false;
                }
            }

            //TODO: proper line-of-sight algorithm
            var startX = (int)Math.Max(targetX - r, 0);
            var endX   = (int)Math.Min(targetX + r, _width - 1);
            var startY = (int)Math.Max(targetY - r, 0);
            var endY   = (int)Math.Min(targetY + r, _height - 1);

           for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    _visible[x, y] = true;
                    _explored[x, y] = true;
                }
            }
        }


    }
}
