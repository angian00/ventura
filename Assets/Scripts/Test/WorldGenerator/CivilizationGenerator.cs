using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Random = UnityEngine.Random;

namespace Ventura.Test.WorldGenerating
{
    public struct FloodFillPos
    {
        public int id;
        public int x;
        public int y;

        public FloodFillPos(int id, int x, int y)
        {
            this.id = id;
            this.x = x;
            this.y = y;
        }
    }


    public class CivilizationGenerator
    {
        public int width;
        public int height;
        public int sectorSize;
        public int wSectors;
        public int hSectors;

        public GameMap gameMap;


        public CivilizationGenerator(GameMap gameMap)
        {
            this.width = gameMap.width;
            this.height = gameMap.height;
            this.sectorSize = gameMap.sectorSize;

            this.wSectors = width / sectorSize;
            this.hSectors = height / sectorSize;

            this.gameMap = gameMap;
        }


        /**
         * As per https://www.ncbi.nlm.nih.gov/pmc/articles/PMC7840081/
         */
        public int[,] GenerateCivilizationsVoronoi(int nCivilizations)
        {
            var civDistribution = new int[width, height];
            var civSeeds = new Dictionary<int, Vector2Int>();

            for (int civId = 1; civId <= nCivilizations; civId++)
            {
                var startPos = findFreePos(civDistribution, gameMap.terrain);
                civDistribution[startPos.x, startPos.y] = civId;
                civSeeds.Add(civId, startPos);
            }

            var areaVertices = new Vector2Int[4] {
                new Vector2Int(0, 0),
                new Vector2Int(width, 0),
                new Vector2Int(width, height),
                new Vector2Int(0, height)
            };

            processVoronoiArea(areaVertices, civDistribution, civSeeds);

            return civDistribution;
        }

        static int nRecurse = 0;
        const int MAX_RECURSE = 20;

        private void processVoronoiArea(Vector2Int[] areaVertices, int[,] civDistribution, Dictionary<int, Vector2Int> civSeeds)
        {
            var xStart = areaVertices[0].x;
            var xEnd = areaVertices[1].x;
            var yStart = areaVertices[0].y;
            var yEnd = areaVertices[3].y;

            if (xStart == xEnd - 1 && yStart == yEnd - 1)
            {
                //single-pixel case
                if (gameMap.terrain[xStart, yStart] == TerrainType.Water)
                    civDistribution[xStart, yStart] = -1;
                else
                    civDistribution[xStart, yStart] = findClosestCiv(areaVertices[0], civSeeds);

                return;
            }

            //DebugUtils.Log($"processVoronoiArea; areaVertices:");
            //foreach (var v in areaVertices)
            //    DebugUtils.Log($"{v.x}, {v.y}");

            //if (nRecurse > MAX_RECURSE)
            //    throw new GameException("Infinite recursion!");

            bool allSameCiv = true;
            int firstCivFound = -1;
            for (int iVertex = 0; iVertex < 4; iVertex++)
            {
                var vertexCiv = findClosestCiv(areaVertices[iVertex], civSeeds);
                if (firstCivFound == -1)
                    firstCivFound = vertexCiv;
                else if (firstCivFound != vertexCiv)
                {
                    allSameCiv = false;
                    break;
                }
            }

            //DebugUtils.Log($"processVoronoiArea; allSameCiv: {allSameCiv}");


            if (allSameCiv)
            {
                //ok, assign the area
                for (int x = areaVertices[0].x; x < areaVertices[1].x; x++)
                {
                    for (int y = areaVertices[0].y; y < areaVertices[3].y; y++)
                    {
                        if (gameMap.terrain[x, y] == TerrainType.Water)
                            civDistribution[x, y] = -1;
                        else
                            civDistribution[x, y] = firstCivFound;
                    }
                }
            }
            else
            {
                nRecurse++;
                //DebugUtils.Log($"recursing; nRecurse= {nRecurse}");

                //recurse
                var xMidpoint = (xEnd + xStart) / 2;
                var yMidpoint = (yEnd + yStart) / 2;

                var topLeft = new Vector2Int(xStart, yStart);
                var topMiddle = new Vector2Int(xMidpoint, yStart);
                var topRight = new Vector2Int(xEnd, yStart);
                var middleLeft = new Vector2Int(xStart, yMidpoint);
                var middleMiddle = new Vector2Int(xMidpoint, yMidpoint);
                var middleRight = new Vector2Int(xEnd, yMidpoint);
                var bottomLeft = new Vector2Int(xStart, yEnd);
                var bottomMiddle = new Vector2Int(xMidpoint, yEnd);
                var bottomRight = new Vector2Int(xEnd, yEnd);

                //when the start area is 1-pixel wide (or tall), do not recurse in both dimensions
                //since integer division rounds down, the check is made on the left (top) sides
                Vector2Int[] recursedAreaVertices;

                if (xMidpoint > xStart && yMidpoint > yStart)
                {
                    //DebugUtils.Log($"will recurse top left");
                    recursedAreaVertices = new Vector2Int[4] { topLeft, topMiddle, middleMiddle, middleLeft };
                    processVoronoiArea(recursedAreaVertices, civDistribution, civSeeds);
                }

                if (yMidpoint > yStart)
                { //DebugUtils.Log($"will recurse top right");
                    recursedAreaVertices = new Vector2Int[4] { topMiddle, topRight, middleRight, middleMiddle };
                    processVoronoiArea(recursedAreaVertices, civDistribution, civSeeds);
                }

                if (xMidpoint > xStart)
                {
                    //DebugUtils.Log($"will recurse bottom left");
                    recursedAreaVertices = new Vector2Int[4] { middleLeft, middleMiddle, bottomMiddle, bottomLeft };
                    processVoronoiArea(recursedAreaVertices, civDistribution, civSeeds);
                }

                //DebugUtils.Log($"will recurse bottom right");
                recursedAreaVertices = new Vector2Int[4] { middleMiddle, middleRight, bottomRight, bottomMiddle };
                processVoronoiArea(recursedAreaVertices, civDistribution, civSeeds);


                nRecurse--;
            }
        }


        private int findClosestCiv(Vector2Int pos, Dictionary<int, Vector2Int> civSeeds)
        {
            var closestDistance2 = width * width + height * height;
            var closestCiv = -1;

            foreach (var civId in civSeeds.Keys)
            {
                var dx = pos.x - civSeeds[civId].x;
                var dy = pos.y - civSeeds[civId].y;
                var distance2 = dx * dx + dy * dy;

                if (distance2 < closestDistance2)
                {
                    closestDistance2 = distance2;
                    closestCiv = civId;
                }

            }

            return closestCiv;
        }


        public int[,] GenerateCivilizationsBySector(int nCivilizations)
        {
            var civSectorDistribution = new int[wSectors, hSectors];
            var posQueue = new List<FloodFillPos>();
            var contendedSectors = new HashSet<Vector2Int>();
            for (int civId = 1; civId <= nCivilizations; civId++)
            {
                var startPos = findFreeSector(civSectorDistribution, gameMap.sectors);
                posQueue.Add(new FloodFillPos(civId, startPos.x, startPos.y));
            }
            while (posQueue.Count > 0)
            {
                var targetIndex = Random.Range(0, posQueue.Count);
                var currPoint = posQueue[targetIndex];
                posQueue.RemoveAt(targetIndex);
                if (civSectorDistribution[currPoint.x, currPoint.y] != 0)
                {
                    if (civSectorDistribution[currPoint.x, currPoint.y] > 0 && civSectorDistribution[currPoint.x, currPoint.y] != currPoint.id)
                        contendedSectors.Add(new Vector2Int(currPoint.x, currPoint.y));
                    continue;
                }
                if (gameMap.sectors[currPoint.x, currPoint.y].landCount == 0)
                {
                    civSectorDistribution[currPoint.x, currPoint.y] = -1;
                    continue;
                }
                civSectorDistribution[currPoint.x, currPoint.y] = currPoint.id;

                //x coordinate wraps, y doesn't
                posQueue.Add(new FloodFillPos(currPoint.id, (currPoint.x - 1 + wSectors) % wSectors, currPoint.y));
                posQueue.Add(new FloodFillPos(currPoint.id, (currPoint.x + 1) % wSectors, currPoint.y));
                if (currPoint.y > 0)
                    posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x, currPoint.y - 1));
                if (currPoint.y < hSectors - 1)
                    posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x, currPoint.y + 1));
            }
            var civDistribution = new int[width, height];
            for (int xSector = 0; xSector < wSectors; xSector++)
            {
                for (int ySector = 0; ySector < hSectors; ySector++)
                {
                    if (civSectorDistribution[xSector, ySector] <= 0)
                        continue;

                    for (int x = xSector * sectorSize; x < (xSector + 1) * sectorSize; x++)
                    {
                        for (int y = ySector * sectorSize; y < (ySector + 1) * sectorSize; y++)
                        {
                            if (gameMap.terrain[x, y] != TerrainType.Water)
                                civDistribution[x, y] = civSectorDistribution[xSector, ySector];
                        }
                    }
                }
            }

            //resolve contended sectors
            foreach (var contendedSector in contendedSectors)
            {
                var xSector = contendedSector.x;
                var ySector = contendedSector.y;

                //reset civ for sector
                for (int x = xSector * sectorSize; x < (xSector + 1) * sectorSize; x++)
                {
                    for (int y = ySector * sectorSize; y < (ySector + 1) * sectorSize; y++)
                    {
                        civDistribution[x, y] = 0;
                    }
                }

                //start from neighbors
                //left and right
                for (int y = ySector * sectorSize; y < (ySector + 1) * sectorSize; y++)
                {
                    var x = (xSector * sectorSize - 1 + width) % width;
                    if (gameMap.terrain[x, y] != TerrainType.Water && civDistribution[x, y] > 0)
                        posQueue.Add(new FloodFillPos(civDistribution[x, y], (x + 1) % width, y));

                    x = ((xSector + 1) * sectorSize) % width;
                    if (gameMap.terrain[x, y] != TerrainType.Water && civDistribution[x, y] > 0)
                        posQueue.Add(new FloodFillPos(civDistribution[x, y], (x - 1 + width) % width, y));
                }

                //top and bottom
                for (int x = xSector * sectorSize; x < (xSector + 1) * sectorSize; x++)
                {
                    var y = ySector * sectorSize - 1;
                    if (y >= 0 && gameMap.terrain[x, y] != TerrainType.Water && civDistribution[x, y] > 0)
                        posQueue.Add(new FloodFillPos(civDistribution[x, y], x, y + 1));

                    y = (ySector + 1) * sectorSize;
                    if (y < height && gameMap.terrain[x, y] != TerrainType.Water && civDistribution[x, y] > 0)
                        posQueue.Add(new FloodFillPos(civDistribution[x, y], x, y - 1));
                }

                //flood fill
                while (posQueue.Count > 0)
                {
                    var targetIndex = Random.Range(0, posQueue.Count);
                    var currPoint = posQueue[targetIndex];
                    posQueue.RemoveAt(targetIndex);

                    //DebugUtils.Log($"flood filling pos {currPoint.x}, {currPoint.y}");
                    if (currPoint.x < xSector * sectorSize || currPoint.x >= (xSector + 1) * sectorSize ||
                            currPoint.y < ySector * sectorSize || currPoint.y >= (ySector + 1) * sectorSize)
                        throw new GameException("GenerateCivilizationsBySector escaped out of sector");

                    if (civDistribution[currPoint.x, currPoint.y] != 0)
                        continue;

                    if (gameMap.terrain[currPoint.x, currPoint.y] == TerrainType.Water)
                    {
                        civDistribution[currPoint.x, currPoint.y] = -1;
                        continue;
                    }

                    civDistribution[currPoint.x, currPoint.y] = currPoint.id;

                    //stay inside sector
                    if (currPoint.x > xSector * sectorSize)
                        posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x - 1, currPoint.y));

                    if (currPoint.x < (xSector + 1) * sectorSize - 1)
                        posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x + 1, currPoint.y));

                    if (currPoint.y > ySector * sectorSize)
                        posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x, currPoint.y - 1));
                    if (currPoint.y < (ySector + 1) * sectorSize - 1)
                        posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x, currPoint.y + 1));
                }
            }

            return civDistribution;
        }


        public int[,] GenerateCivilizationsFloodFill(int nCivilizations)
        {
            const int maxCivArea = 10000;

            var civDistribution = new int[width, height];
            var posQueue = new List<FloodFillPos>();
            //var posQueue = new Queue<FloodFillPos>();

            for (int civId = 1; civId <= nCivilizations; civId++)
            {
                //var currCivArea = 0;

                var startPos = findFreePos(civDistribution, gameMap.terrain);
                posQueue.Add(new FloodFillPos(civId, startPos.x, startPos.y));
                //posQueue.Enqueue(new FloodFillPos(id, startPos.x, startPos.y));
            }


            while (posQueue.Count > 0)
            {
                var targetIndex = Random.Range(0, posQueue.Count);
                var currPoint = posQueue[targetIndex];
                posQueue.RemoveAt(targetIndex);
                //var currPoint = posQueue.Dequeue();

                if (civDistribution[currPoint.x, currPoint.y] != 0)
                    continue;

                if (gameMap.terrain[currPoint.x, currPoint.y] == TerrainType.Water)
                {
                    civDistribution[currPoint.x, currPoint.y] = -1;
                    continue;
                }

                civDistribution[currPoint.x, currPoint.y] = currPoint.id;
                //currCivArea++;
                //if (currCivArea >= maxCivArea)
                //    break;

                posQueue.Add(new FloodFillPos(currPoint.id, (currPoint.x - 1 + width) % width, currPoint.y));
                posQueue.Add(new FloodFillPos(currPoint.id, (currPoint.x + 1) % width, currPoint.y));

                if (currPoint.y > 0)
                    posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x, currPoint.y - 1));
                if (currPoint.y < height - 1)
                    posQueue.Add(new FloodFillPos(currPoint.id, currPoint.x, currPoint.y + 1));
                //posQueue.Enqueue(new FloodFillPos(currPoint.id, (currPoint.x - 1 + width) % width, currPoint.y));
                //posQueue.Enqueue(new FloodFillPos(currPoint.id, (currPoint.x + 1) % width, currPoint.y));
                //posQueue.Enqueue(new FloodFillPos(currPoint.id, currPoint.x, (currPoint.y - 1 + height) % height));
                //posQueue.Enqueue(new FloodFillPos(currPoint.id, currPoint.x, (currPoint.y + 1) % height));
            }

            return civDistribution;
        }


        private Vector2Int findFreePos(int[,] civDistribution, TerrainType[,] terrain)
        {
            while (true)
            {
                var x = Random.Range(0, width);
                var y = Random.Range(0, height);

                if (civDistribution[x, y] == 0 && terrain[x, y] != TerrainType.Water)
                    return new Vector2Int(x, y);
            }
        }

        private Vector2Int findFreeSector(int[,] civDistribution, MapSector[,] sectors)
        {
            while (true)
            {
                var x = Random.Range(0, wSectors);
                var y = Random.Range(0, hSectors);

                if (civDistribution[x, y] == 0 && sectors[x, y].landCount > 0)
                    return new Vector2Int(x, y);
            }
        }
    }

}
