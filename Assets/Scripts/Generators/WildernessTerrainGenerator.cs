using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Generators
{

    public class WildernessTerrainGenerator
    {

        public static TerrainDef[,] generateTerrain(int width, int height)
        {
            const float noiseXOffset = 0.0f;
            const float noiseYOffset = 0.0f;
            const float noiseScale = 10.0f;


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


            var terrainMap = new TerrainDef[width, height];

            //stats collection
            var maxTerrain = 0;
            var maxNoise = -1.0f;
            int[] countTerrain = new int[terrainTypes.Length];
            //

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var noiseX = noiseXOffset + noiseScale * x / width;
                    var noiseY = noiseYOffset + noiseScale * y / height;

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

                    terrainMap[x, y] = terrainTypes[iTerrain - 1];
                }
            }

            //GameDebugging.Log($"DEBUG - maxTerrain: {maxTerrain}, maxPerlin: {maxNoise}");
            //for (var i=0; i < countTerrain.Length; i++)
            //{
            //    GameDebugging.Log($"DEBUG - countTerrain[{i}]: {countTerrain[i]}");
            //}

            return terrainMap;
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
    }

}