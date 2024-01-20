using System.Collections.Generic;
using UnityEngine;

namespace Ventura.Test.WorldGenerating
{
    public class CivilizationGenerator
    {
        private struct CivilizationPoint
        {
            public int civId;
            public int x;
            public int y;

            public CivilizationPoint(int civId, int x, int y)
            {
                this.civId = civId;
                this.x = x;
                this.y = y;
            }
        }


        public int width;
        public int height;

        public CivilizationGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void GenerateCivilizations(GameMap gameMap, int nCivilizations)
        {
            const int maxCivArea = 10000;

            var civDistribution = new int[gameMap.width, gameMap.height];
            var posQueue = new List<CivilizationPoint>();

            for (int civId = 1; civId <= nCivilizations; civId++)
            {
                //var currCivArea = 0;

                var startPos = findFreePos(civDistribution, gameMap.terrain);
                posQueue.Add(new CivilizationPoint(civId, startPos.x, startPos.y));
            }


            while (posQueue.Count > 0)
            {
                var targetIndex = Random.Range(0, posQueue.Count);
                var currPoint = posQueue[targetIndex];
                posQueue.RemoveAt(targetIndex);

                if (civDistribution[currPoint.x, currPoint.y] != 0)
                    continue;

                if (gameMap.terrain[currPoint.x, currPoint.y] == TerrainType.Water)
                {
                    civDistribution[currPoint.x, currPoint.y] = -1;
                    continue;
                }

                civDistribution[currPoint.x, currPoint.y] = currPoint.civId;
                //currCivArea++;
                //if (currCivArea >= maxCivArea)
                //    break;

                posQueue.Add(new CivilizationPoint(currPoint.civId, (currPoint.x - 1 + width) % width, currPoint.y));
                posQueue.Add(new CivilizationPoint(currPoint.civId, (currPoint.x + 1) % width, currPoint.y));
                posQueue.Add(new CivilizationPoint(currPoint.civId, currPoint.x, (currPoint.y - 1 + height) % height));
                posQueue.Add(new CivilizationPoint(currPoint.civId, currPoint.x, (currPoint.y + 1) % height));
            }

            gameMap.civilizations = civDistribution;
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
    }

}
