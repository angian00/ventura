using System;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;
using Random = UnityEngine.Random;

namespace Ventura.Generators
{

    public class MapGenerator
    {

        public static GameMap GenerateWildernessMap(int nRows, int nCols, string? mapName = null, bool hasSites = true)
        {
            DebugUtils.Log("GenerateWildernessMap()");

            if (mapName == null)
                mapName = FileStringGenerator.Sites.GenerateString();

            var newMap = new GameMap(mapName, mapName, nRows, nCols);

            generateTerrain(newMap);

            if (hasSites)
                addSites(newMap, 8);

            //choose starting pos on a empty square
            while (true)
            {
                int x = Random.Range(0, nRows);
                int y = Random.Range(0, nCols);
                if (newMap.Terrain[x, y].Walkable && (newMap.GetAnyEntityAt<Entity>(x, y) == null))
                {
                    newMap.StartingPos = new Vector2Int(x, y);
                    break;
                }
            }

            return newMap;
        }

        private static void generateTerrain(GameMap targetMap)
        {
            float noiseXOffset = 0.0f;
            float noiseYOffset = 0.0f;
            float noiseScale = 10.0f;


            //float[] terrainLevels = new float[] { 0.0f, 0.4f, 0.65f, 0.85f, .99f, 1.0f };
            float[] terrainLevels = new float[] { 0.0f, 0.40f, 0.62f, 0.80f, .90f, 1.0f };
            TerrainType[] terrainTypes = new TerrainType[] {
                TerrainType.Plains1,
                TerrainType.Plains2,
                TerrainType.Hills1,
                TerrainType.Hills2,
                TerrainType.Mountains,
            };


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


        private static void addSites(GameMap targetMap, int nSites)
        {
            DebugUtils.Log($"addSite({targetMap.Name}, {nSites})");

            const int MIN_SITE_DISTANCE = 3;

            var i = 0;
            while (i < nSites)
            {
                //leave a 1 tile padding around the borders
                var x = Random.Range(1, targetMap.Width - 1);
                var y = Random.Range(1, targetMap.Height - 1);

                //if not any (entity.x == x and entity.y == y for entity in target_map.entities):

                // guarantee minimal distance between sites
                var isTileOk = targetMap.Terrain[x, y].Walkable;
                foreach (var e in targetMap.Entities)
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
                newSite.MoveTo(x, y);

                targetMap.Entities.Add(newSite);

                i++;
            }
        }
    }
}