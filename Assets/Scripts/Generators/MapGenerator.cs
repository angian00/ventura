using System;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Entities;
using Ventura.Util;
using Random = UnityEngine.Random;

namespace Ventura.Generators
{

    public class MapGenerator
    {

        public static GameMap GenerateWildernessMap(int nRows, int nCols, string? mapName = null, bool doGenerateSites = true)
        {
            DebugUtils.Log("GenerateWildernessMap()");

            if (mapName == null)
                mapName = FileStringGenerator.Sites.GenerateString();

            var newMap = new GameMap(mapName, mapName, nRows, nCols);

            generateTerrain(newMap);
            if (doGenerateSites)
                generateSites(newMap, 15);
            //generateSites(newMap, 1);

            generateSomeItems(newMap);
            generateSomeMonsters(newMap);
            //generateSomeItems(newMap, 1);
            //generateSomeMonsters(newMap, 1);

            newMap.StartingPos = RandomUtils.RandomEmptyPos(newMap);

            return newMap;
        }

        private static void generateTerrain(GameMap targetMap)
        {
            float noiseXOffset = 0.0f;
            float noiseYOffset = 0.0f;
            float noiseScale = 10.0f;


            var terrainWeights = new float[] { 40, 40, 22, 18, 10, 10 };
            TerrainDef[] terrainTypes = new TerrainDef[] {
                TerrainDef.Water,
                TerrainDef.Plains1,
                TerrainDef.Plains2,
                TerrainDef.Hills1,
                TerrainDef.Hills2,
                TerrainDef.Mountains,
            };

            var terrainLevels = computeTerrainLevels(terrainWeights);


            //stats collection
            var maxTerrain = 0;
            var maxNoise = -1.0f;
            int[] countTerrain = new int[terrainTypes.Length];
            //

            for (var x = 0; x < targetMap.Width; x++)
            {
                for (var y = 0; y < targetMap.Height; y++)
                {
                    var noiseX = noiseXOffset + noiseScale * x / targetMap.Width;
                    var noiseY = noiseYOffset + noiseScale * y / targetMap.Height;

                    var noiseVal = Mathf.PerlinNoise(noiseX, noiseY);
                    //GameDebugging.Log($"noiseVal (x/y): {noiseVal}");
                    if (noiseVal > maxNoise)
                        maxNoise = noiseVal;

                    int iTerrain;
                    for (iTerrain = 0; iTerrain < terrainLevels.Length; iTerrain++)
                    {
                        if (noiseVal < terrainLevels[iTerrain])
                            break;
                    }

                    //stats collection
                    if (iTerrain > maxTerrain)
                        maxTerrain = iTerrain;
                    countTerrain[iTerrain - 1]++;
                    //

                    targetMap.Terrain[x, y] = terrainTypes[iTerrain - 1];
                }
            }

            //GameDebugging.Log($"DEBUG - maxTerrain: {maxTerrain}, maxPerlin: {maxNoise}");
            //for (var i=0; i < countTerrain.Length; i++)
            //{
            //    GameDebugging.Log($"DEBUG - countTerrain[{i}]: {countTerrain[i]}");
            //}
        }

        private static float[] computeTerrainLevels(float[] terrainWeights)
        {
            var res = new float[terrainWeights.Length + 1];
            res[0] = 0.0f;

            float totWeight = 0.0f;
            for (var i = 0; i < terrainWeights.Length; i++)
                totWeight += terrainWeights[i];

            for (var i = 0; i < terrainWeights.Length; i++)
                res[i + 1] = res[i] + terrainWeights[i] / totWeight;

            return res;
        }

        private static void generateSites(GameMap targetMap, int nSites)
        {
            //DebugUtils.Log($"generateSites({targetMap.Name}, {nSites})");

            const int MIN_SITE_DISTANCE = 3;

            var i = 0;
            while (i < nSites)
            {
                //leave a 1 tile padding around the borders
                var x = Random.Range(1, targetMap.Width - 1);
                var y = Random.Range(1, targetMap.Height - 1);

                // guarantee minimal distance between sites
                var isTileOk = targetMap.Terrain[x, y].Walkable;
                foreach (var e in targetMap.GetAllEntities<Entity>())
                {
                    if ((e.x == x && e.y == y) || (e is Site && Math.Abs(e.x - x) < MIN_SITE_DISTANCE && Math.Abs(e.y - y) < MIN_SITE_DISTANCE))
                    {
                        isTileOk = false;
                        break;
                    }
                }

                if (!isTileOk)
                    continue;


                var siteName = FileStringGenerator.Sites.GenerateString();
                var newSite = new Site(siteName, targetMap.Name);

                var pos = new Vector2Int(x, y);

                targetMap.AddEntity(newSite, pos);

                i++;
            }
        }

        private static void generateSomeItems(GameMap targetMap, int nItems = -1)
        {
            const float perc = .02f;

            if (nItems == -1)
                nItems = (int)(perc * targetMap.Width * targetMap.Height);

            var items = GameItemGenerator.Instance.GenerateItems(nItems);
            foreach (var item in items)
            {
                var pos = RandomUtils.RandomEmptyPos(targetMap);

                targetMap.AddEntity(item, pos);
            }

        }


        private static void generateSomeMonsters(GameMap targetMap, int nMonsters = -1)
        {
            const float perc = .01f;

            if (nMonsters == -1)
                nMonsters = (int)(perc * targetMap.Width * targetMap.Height);

            var monsters = MonsterGenerator.Instance.GenerateMonsters(nMonsters);
            foreach (var monster in monsters)
            {
                var pos = RandomUtils.RandomEmptyPos(targetMap);

                targetMap.AddEntity(monster, pos);
            }
        }
    }

}