using System.Collections.Generic;
using UnityEngine;

namespace Ventura.Test.WorldGenerating
{
    public enum TerrainType
    {
        Water,
        Desert,
        Grass,
        Forest,
        Tropical,
        Rock,
        Snow,
    }


    public class MapSector
    {
        public int landCount;
    }

    public class IslandInfo
    {
        public int id;
        public int area;
        public Vector2Int boundingBoxTopLeft;
        public Vector2Int boundingBoxBottomRight;
        public bool isCivilized;
    }


    public class GameMap
    {
        public int sectorSize = 10;

        public int width;
        public int height;
        public float[,] altitudes;
        public float[,] temperatures;
        public float[,] moistures;
        public TerrainType[,] terrain;
        public MapSector[,] sectors;

        public int[,] civilizations;
        public int[,] islandIds;
        public List<IslandInfo> islands;


        public GameMap(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void ComputeSectors()
        {
            var wSectors = width / sectorSize;
            var hSectors = height / sectorSize;

            sectors = new MapSector[wSectors, hSectors];

            for (int xSectors = 0; xSectors < wSectors; xSectors++)
            {
                for (int ySectors = 0; ySectors < hSectors; ySectors++)
                {
                    var sector = new MapSector();
                    int landCount = 0;
                    for (int x = xSectors * sectorSize; x < (xSectors + 1) * sectorSize; x++)
                    {
                        for (int y = ySectors * sectorSize; y < (ySectors + 1) * sectorSize; y++)
                        {
                            if (terrain[x, y] != TerrainType.Water)
                                landCount++;
                        }
                    }

                    sector.landCount = landCount;
                    sectors[xSectors, ySectors] = sector;
                }
            }
        }

        public void ComputeIslands()
        {
            islandIds = new int[width, height];
            islands = new();
            var currIslandId = 1;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (islandIds[x, y] > 0)
                        continue;

                    if (terrain[x, y] == TerrainType.Water)
                    {
                        islandIds[x, y] = -1;
                        continue;
                    }

                    var minX = int.MaxValue;
                    var maxX = int.MinValue;
                    var minY = int.MaxValue;
                    var maxY = int.MinValue;
                    var area = 0;
                    bool isCivilized = false;

                    //flood fill
                    var posQueue = new Stack<Vector2Int>();

                    posQueue.Push(new Vector2Int(x, y));


                    while (posQueue.Count > 0)
                    {
                        var currPoint = posQueue.Pop();

                        if (islandIds[currPoint.x, currPoint.y] != 0)
                            continue;

                        if (terrain[currPoint.x, currPoint.y] == TerrainType.Water)
                        {
                            islandIds[currPoint.x, currPoint.y] = -1;
                            continue;
                        }

                        islandIds[currPoint.x, currPoint.y] = currIslandId;
                        //FIXME: bounding box when x wraps around
                        if (currPoint.x < minX)
                            minX = currPoint.x;
                        if (currPoint.x > maxX)
                            maxX = currPoint.x;

                        if (currPoint.y < minY)
                            minY = currPoint.y;
                        if (currPoint.y > maxY)
                            maxY = currPoint.y;

                        area++;

                        if (civilizations[currPoint.x, currPoint.y] > 0)
                            isCivilized = true;

                        posQueue.Push(new Vector2Int((currPoint.x - 1 + width) % width, currPoint.y));
                        posQueue.Push(new Vector2Int((currPoint.x + 1) % width, currPoint.y));

                        if (currPoint.y > 0)
                            posQueue.Push(new Vector2Int(currPoint.x, currPoint.y - 1));
                        if (currPoint.y < height - 1)
                            posQueue.Push(new Vector2Int(currPoint.x, currPoint.y + 1));
                    }

                    var currIslandInfo = new IslandInfo();
                    currIslandInfo.id = currIslandId;
                    currIslandInfo.boundingBoxTopLeft = new Vector2Int(minX, minY);
                    currIslandInfo.boundingBoxBottomRight = new Vector2Int(maxX, maxY);
                    currIslandInfo.area = area;
                    currIslandInfo.isCivilized = isCivilized;
                    islands.Add(currIslandInfo);

                    currIslandId++;
                }
            }
        }
    }
}
