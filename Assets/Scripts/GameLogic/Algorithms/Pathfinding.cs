
// Algorithm adapted from https://github.com/pixelfac/2D-Astar-Pathfinding-in-Unity 
// by Nathan Harris (https://github.com/pixelfac).
// Licensed under https://opensource.org/license/mit/

namespace Ventura.GameLogic.Algorithms
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Ventura.Util;

    public class Pathfinding
    {
        private int _w;
        private int _h;
        private PathNode[,] _pathNodes;


        private class PathNode
        {
            public Vector2Int pos;
            public bool isBlocking;

            public PathNode? parent;
            public float FCost;
            public float gCost;
            public float hCost;


            public PathNode(Vector2Int pos, bool isBlocking)
            {
                this.pos = pos;
                this.isBlocking = isBlocking;
            }
        }


        public Pathfinding(bool[,] blockingTiles)
        {
            this._w = blockingTiles.GetLength(0);
            this._h = blockingTiles.GetLength(1);

            _pathNodes = new PathNode[_w, _h];

            for (int x = 0; x < _w; x++)
                for (int y = 0; y < _h; y++)
                    _pathNodes[x, y] = new PathNode(new Vector2Int(x, y), blockingTiles[x, y]);
        }


        public List<Vector2Int> FindPathAStar(Vector2Int startPos, Vector2Int targetPos)
        {
            DebugUtils.Log("Pathfinding.FindPathAStar");

            var seekerNode = _pathNodes[startPos.x, startPos.y];
            var targetNode = _pathNodes[targetPos.x, targetPos.y];


            List<PathNode> openSet = new List<PathNode>();
            HashSet<PathNode> closedSet = new HashSet<PathNode>();

            openSet.Add(seekerNode);

            //calculates path for pathfinding
            while (openSet.Count > 0)
            {
                //iterates through openSet and finds lowest FCost
                PathNode node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost <= node.FCost)
                    {
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                //if target found, retrace path
                if (node == targetNode)
                    return retracePath(seekerNode, targetNode);


                //adds neighbor nodes to openSet
                foreach (var neighbour in getNeighbors(node))
                {
                    if (neighbour.isBlocking || closedSet.Contains(neighbour))
                        continue;

                    float newCostToNeighbour = node.gCost + getDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = getDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            throw new GameException("FindPathAStar failed");
        }


        private List<PathNode> getNeighbors(PathNode node)
        {
            var neighbors = new List<PathNode>();

            //top
            if (node.pos.y < _h - 1)
            {
                neighbors.Add(_pathNodes[node.pos.x, node.pos.y + 1]);

                //top-right
                if (node.pos.x < _w - 1)
                    neighbors.Add(_pathNodes[node.pos.x + 1, node.pos.y + 1]);

                //top-left
                if (node.pos.x > 0)
                    neighbors.Add(_pathNodes[node.pos.x - 1, node.pos.y + 1]);
            }

            //bottom
            if (node.pos.y > 0)
            {
                neighbors.Add(_pathNodes[node.pos.x, node.pos.y - 1]);

                //bottom-right
                if (node.pos.x < _w - 1)
                    neighbors.Add(_pathNodes[node.pos.x + 1, node.pos.y - 1]);

                //bottom-left
                if (node.pos.x > 0)
                    neighbors.Add(_pathNodes[node.pos.x - 1, node.pos.y - 1]);
            }

            //right
            if (node.pos.x < _w - 1)
                neighbors.Add(_pathNodes[node.pos.x + 1, node.pos.y]);

            //left
            if (node.pos.x > 0)
                neighbors.Add(_pathNodes[node.pos.x - 1, node.pos.y]);


            return neighbors;
        }




        //reverses calculated path so first node is closest to seeker
        private List<Vector2Int> retracePath(PathNode startNode, PathNode endNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            PathNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.pos);
                currentNode = currentNode.parent;
            }

            path.Add(startNode.pos);
            path.Reverse();

            return path;
        }


        //gets distance between 2 nodes for calculating cost
        private float getDistance(PathNode nodeA, PathNode nodeB)
        {
            int distX = Math.Abs(nodeA.pos.x - nodeB.pos.x);
            int distY = Math.Abs(nodeA.pos.y - nodeB.pos.y);

            // heuristic for rectangular grid with diagonal movement allowed;
            // diagonal movement is still slightly discouraged to avoid weird paths
            int distMax = Math.Max(distX, distY);
            int distMin = Math.Min(distX, distY);
            const float diagWeight = 1.01f;

            return diagWeight * distMin + (distMax - distMin);
        }
    }
}
