using System;
using UnityEngine;
using Ventura.Util;

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


    public class GameMap
    {
        public int width;
        public int height;
        public int[,] altitudes;
        public int[,] latitudes;
        public int[,] temperatures;
        public int[,] moistures;
        public TerrainType[,] terrain;


        public GameMap(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }


    public class WorldGenerator
    {
        //public static int MAX_ALTITUDE = 4; //values go from 0 to MAX_ALTITUDE
        public static int MAX_ALTITUDE = 10;
        public static int MAX_LATITUDE = 10; //values go from -MAX_LATITUDE to +MAX_LATITUDE
        public static int MAX_TEMPERATURE = 10;
        public static int MAX_MOISTURE = 10;


        public static GameMap GenerateWorld(int width, int height)
        {
            var gameMap = new GameMap(width, height);
            gameMap.altitudes = generateAltitudes(width, height);
            gameMap.latitudes = computeLatitudes(width, height);
            gameMap.temperatures = computeTemperatures(gameMap.altitudes, gameMap.latitudes);

            var water = generateWater(gameMap.altitudes);
            gameMap.moistures = generateMoistures(water, gameMap.latitudes);

            gameMap.terrain = computeBiomes(gameMap.temperatures, gameMap.moistures);

            return gameMap;
        }

        private static int[,] generateAltitudes(int width, int height)
        {
            DebugUtils.Log("generateAltitudes");

            //var noiseFrequencies = new float[] { .4f, .8f, 1.6f };
            //var noiseAmplitudes = new float[] { 0.6f, 0.4f, 0.2f };
            var noiseFrequencies = new float[] { 1f };
            var noiseAmplitudes = new float[] { 1f };
            //var quantizationLevels = new float[] { 0.65f, 0.68f, 0.7f, 0.74f, 0.78f, 0.85f, 0.9f, 0.95f, 0.98f, 0.99f };
            var quantizationLevels = new float[] { 0.60f, 0.65f, 0.7f, 0.74f, 0.78f, 0.85f, 0.9f, 0.95f, 0.98f, 0.99f };

            return generateNoise(width, height, true, noiseFrequencies, noiseAmplitudes, quantizationLevels);
        }


        private static int seedProg = 101;
        private static int[,] generateNoise(int width, int height, bool isWrapped, float[] noiseFrequencies, float[] noiseAmplitudes, float[] quantizationLevels)
        {
            //stats collection
            var maxNoise = -1.0f;
            var cumNoise = 0.0f;
            //


            var res = new int[width, height];
            var seed = (int)Math.Round(UnityEngine.Random.value * 100_000);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    float noiseVal = 0.0f;
                    for (var iOctave = 0; iOctave < noiseFrequencies.Length; iOctave++)
                    {
                        if (isWrapped)
                        {
                            var s = noiseFrequencies[iOctave] * x / width;
                            var t = noiseFrequencies[iOctave] * y / height;
                            var nx = Math.Cos(s * 2 * MathF.PI) / (Mathf.PI);
                            var ny = Math.Cos(t * 2 * MathF.PI) / (Mathf.PI);
                            var nz = Math.Sin(s * 2 * MathF.PI) / (Mathf.PI);
                            var nw = Math.Sin(t * 2 * MathF.PI) / (Mathf.PI);

                            noiseVal += noiseAmplitudes[iOctave] * (0.5f + OpenSimplex2S.Noise4_Fallback(seed, nx, ny, nz, nw));
                        }
                        else
                        {

                            var noiseX = noiseFrequencies[iOctave] * x / width;
                            var noiseY = noiseFrequencies[iOctave] * y / height;
                            //noiseVal += noiseAmplitudes[iOctave] * Mathf.PerlinNoise(noiseX, noiseY
                            noiseVal += noiseAmplitudes[iOctave] * (0.5f + OpenSimplex2S.Noise2(seed, noiseX, noiseY));
                        }
                    }

                    //DebugUtils.Log($"noiseVal ({x}, {y}): {noiseVal}");
                    if (noiseVal > maxNoise)
                        maxNoise = noiseVal;
                    cumNoise += noiseVal;

                    var altitude = quantizeNoise(noiseVal, quantizationLevels);


                    res[x, y] = altitude;
                }
            }

            seedProg += 10000;

            var countValues = DataUtils.IntValueStats(res, quantizationLevels.Length);
            DebugUtils.Log($"DEBUG generateNoise - maxNoise: {maxNoise}");
            for (var i = 0; i < countValues.Length; i++)
            {
                DebugUtils.Log($"DEBUG - countValues [{i}]: {countValues[i]}");
            }

            DebugUtils.Log($"DEBUG - average noise: {cumNoise / (width * height)}");


            return res;
        }


        private static int[,] computeLatitudes(int width, int height)
        {
            var latitudes = new int[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    latitudes[x, y] = (int)Math.Round((2.0f * y / height - 1.0f) * MAX_LATITUDE);
                }
            }

            return latitudes;
        }

        private static int[,] computeTemperatures(int[,] altitudes, int[,] latitudes)
        {
            var altWeight = 1.0f;
            var latWeight = 1.0f;

            var w = altitudes.GetLength(0);
            var h = altitudes.GetLength(1);
            var temperatures = new int[w, h];

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var altValue = altWeight * (altitudes[x, y] == 0 ? 3 : altitudes[x, y]); //water cools down temperatures
                    var latValue = latWeight * Math.Abs(latitudes[x, y]);
                    temperatures[x, y] = MAX_TEMPERATURE - (int)Math.Floor((altValue + latValue) / (altWeight + latWeight));
                }
            }

            return temperatures;
        }


        private static bool[,] generateWater(int[,] altitudes)
        {
            DebugUtils.Log("generateWater");

            var w = altitudes.GetLength(0);
            var h = altitudes.GetLength(1);
            var water = new bool[w, h];

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    if (altitudes[x, y] == 0)
                        water[x, y] = true;
                    else
                        water[x, y] = false;
                }
            }

            //TODO: generate lakes noise
            //highAltitudeLakeNoise = PerlinNoise(x * highAltitudeLakeScale, y * highAltitudeLakeScale)
            // maxHighAltitude = 0.8  # Altitudine massima consentita per laghi in altitudine elevata
            // if altitude > seaThreshold and altitude < maxHighAltitude and highAltitudeLakeNoise > highAltitudeLakeThreshold:
            //     terrainType = "lago"

            return water;
        }


        private static int[,] generateMoistures(bool[,] water, int[,] latitudes)
        {
            DebugUtils.Log("generateMoistures");

            var w = water.GetLength(0);
            var h = water.GetLength(1);
            var moistures = new int[w, h];

            var noiseFrequencies = new float[] { .4f, .8f, 1.6f, 3.2f };
            var noiseAmplitudes = new float[] { 0.6f, 0.4f, 0.2f, 0.1f };
            var quantizationLevels = new float[] { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };

            var randomMoistures = generateNoise(w, h, true, noiseFrequencies, noiseAmplitudes, quantizationLevels);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {

                    if (water[x, y])
                        moistures[x, y] = MAX_MOISTURE;
                    else
                    {
                        int baseMoisture = (int)Math.Round(MAX_MOISTURE / 2.5f) - (int)Math.Round(Math.Abs(latitudes[x, y]) / 3.0f);

                        if (DataUtils.CountMatchingNeighbours(water, true, x, y) > 0)
                            //close to water
                            baseMoisture += 2;

                        moistures[x, y] = baseMoisture + randomMoistures[x, y];
                    }
                }
            }

            return moistures;
        }


        private static TerrainType[,] computeBiomes(int[,] temperatures, int[,] moistures)
        {
            const int M_0 = 3;
            const int M_1 = 4;
            const int M_2 = 5;
            const int M_3 = 6;
            const int M_4 = 9;

            const int T_0 = 4;
            const int T_1 = 5;
            const int T_2 = 7;

            // Adapted from https://github.com/GrandPiaf/Biome-and-Vegetation-PCG
            //       
            //       moisture -->
            //   temperature
            //    |
            //    |
            //    v
            //       0              M_0 M_1  M_2  M_3          M_4 
            //     0 ┌───────────────────┬──────────────────────┬───────┐
            //       │                .  │    .   .             │       │
            //       │        Rock    .  │    .  Snow           │       │
            //       │                .  │    .   .             │       │
            //   T_0 ├───────────────────┼──────────────────────┤       │
            //       │                .  │    .   .             │       │
            //       │       Grass    .  │    .  Forest         │       │
            //       │                .  │    .   .             │       │
            //   T_1 ├───────────────────┴────┬─────────────────┤       │
            //       │                .       │   .             │ Water │
            //       │          Grass .       │   .  Forest     │       │
            //       │                .       │   .             │       │
            //   T_2 ├────────────────┬───────┴───┬─────────────┤       │
            //       │                │           │             │       │
            //       │     Desert     │   Grass   │  Tropical   │       │
            //       │                │           │             │       │
            //       └────────────────┴───────────┴─────────────┴───────┘

            var w = temperatures.GetLength(0);
            var h = temperatures.GetLength(1);
            var terrain = new TerrainType[w, h];

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var t = temperatures[x, y];
                    var m = moistures[x, y];

                    if (m >= M_4)
                        terrain[x, y] = TerrainType.Water;
                    else
                    {
                        if (t < T_0)
                        {
                            if (m < M_1)
                                terrain[x, y] = TerrainType.Rock;
                            else
                                terrain[x, y] = TerrainType.Snow;
                        }
                        else if (t < T_1)
                        {
                            if (m < M_1)
                                terrain[x, y] = TerrainType.Grass;
                            else
                                terrain[x, y] = TerrainType.Forest;
                        }
                        else if (t < T_2)
                        {
                            if (m < M_2)
                                terrain[x, y] = TerrainType.Grass;
                            else
                                terrain[x, y] = TerrainType.Forest;
                        }
                        else
                        {
                            if (m < M_0)
                                terrain[x, y] = TerrainType.Desert;
                            else if (m < M_3)
                                terrain[x, y] = TerrainType.Grass;
                            else
                                terrain[x, y] = TerrainType.Tropical;
                        }
                    }
                }
            }

            return terrain;
        }


        private static int quantizeNoise(float noiseVal, float[] quantizationLevels)
        {
            var totalVal = noiseVal;

            int iLevel;
            for (iLevel = 0; iLevel < quantizationLevels.Length; iLevel++)
            {
                if (totalVal < quantizationLevels[iLevel])
                    break;
            }

            return iLevel;
        }


    }
}
