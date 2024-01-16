using UnityEngine;
using Ventura.Util;
using Random = UnityEngine.Random;

namespace Ventura.Test.WorldGenerating
{
    public class GameMap
    {
        public int width;
        public int height;
        public int[,] altitudes;


        public GameMap(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.altitudes = new int[width, height];
        }
    }


    public class WorldGenerator
    {
        public static int N_ALTITUDES = 5;

        public static GameMap GenerateWorld(int width, int height)
        {
            var gameMap = new GameMap(width, height);
            gameMap.altitudes = generateAltitudes(width, height);

            return gameMap;
        }


        private static int[,] generateAltitudes(int width, int height)
        {
            //var noiseFrequencies = new float[] { 4, 1, 0.4f, 0.2f };
            var noiseFrequencies = new float[] { 4, 8, 16 };
            //var noiseAmplitudes = new float[] { 0.6f, 0.3f, 0.2f, 0.2f };
            var noiseAmplitudes = new float[] { 0.8f, 0.4f, 0.1f };

            var noiseXOffset = width * Random.value;
            var noiseYOffset = height * Random.value;

            //stats collection
            var maxAltitude = 0;
            var maxNoise = -1.0f;
            var cumNoise = 0.0f;
            var countFrequencies = new int[N_ALTITUDES];

            //

            var altitudes = new int[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    float noiseVal = 0.0f;
                    for (var iOctave = 0; iOctave < noiseFrequencies.Length; iOctave++)
                    {
                        //wrap around coordinates for a "spherical" world
                        var xWrapped = (x + width / 2) % width;
                        var yWrapped = (y + height / 2) % height;

                        var noiseX = noiseXOffset + noiseFrequencies[iOctave] * xWrapped / width;
                        var noiseY = noiseYOffset + noiseFrequencies[iOctave] * yWrapped / height;

                        noiseVal += noiseAmplitudes[iOctave] * Mathf.PerlinNoise(noiseX, noiseY);
                    }

                    //DebugUtils.Log($"noiseVal ({x}, {y}): {noiseVal}");
                    if (noiseVal > maxNoise)
                        maxNoise = noiseVal;
                    cumNoise += noiseVal;

                    var altitude = rescaleAltitude(noiseVal);

                    //stats collection
                    if (altitude > maxAltitude)
                        maxAltitude = altitude;
                    countFrequencies[altitude]++;
                    //

                    altitudes[x, y] = altitude;
                }
            }

            DebugUtils.Log($"DEBUG - maxTerrain: {maxAltitude}, maxPerlin: {maxNoise}");
            for (var i = 0; i < countFrequencies.Length; i++)
            {
                DebugUtils.Log($"DEBUG - countFrequencies[{i}]: {countFrequencies[i]}");
            }

            DebugUtils.Log($"DEBUG - average noise: {cumNoise / (width * height)}");

            return altitudes;
        }

        private static int rescaleAltitude(float noiseVal)
        {
            float[] altitudeLevels = new float[] { 0.72f, 0.8f, 0.85f, 0.9f };
            Debug.Assert(altitudeLevels.Length == N_ALTITUDES - 1);

            var totalVal = 0.4f + noiseVal * 0.5f;

            int iLevel;
            for (iLevel = 0; iLevel < altitudeLevels.Length; iLevel++)
            {
                if (totalVal < altitudeLevels[iLevel])
                    break;
            }

            return iLevel;
        }


    }
}
