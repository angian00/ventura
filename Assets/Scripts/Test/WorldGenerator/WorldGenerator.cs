using UnityEngine;
using Ventura.Util;
using Random = UnityEngine.Random;

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
        public static int MAX_ALTITUDE = 4; //values go from 0 to MAX_ALTITUDE
        //public static int MAX_ALTITUDE = 10; //values go from 0 to MAX_ALTITUDE
        public static int MAX_LATITUDE = 10; //values go from -MAX_LATITUDE to +MAX_LATITUDE
        public static int MAX_TEMPERATURE = 10; //values go from 0 to MAX_TEMPERATURE


        public static GameMap GenerateWorld(int width, int height)
        {
            var gameMap = new GameMap(width, height);
            gameMap.altitudes = generateAltitudes(width, height);
            gameMap.latitudes = generateLatitudes(width, height);
            gameMap.temperatures = computeTemperatures(gameMap.altitudes, gameMap.latitudes);

            //var water = altitudes == 0 || generateLakes();
            //gameMap.moistures = generateMoistures(water);

            //gameMap.terrain = computeBiomes(temperature, humidity);

            return gameMap;
        }


        private static int[,] generateAltitudes(int width, int height)
        {
            const bool isWrapped = true;

            var noiseFrequencies = new float[] { 4, 8, 16 };
            var noiseAmplitudes = new float[] { 0.8f, 0.4f, 0.1f };

            var noiseXOffset = width * Random.value;
            var noiseYOffset = height * Random.value;

            //stats collection
            var maxAltitude = 0;
            var maxNoise = -1.0f;
            var cumNoise = 0.0f;
            var countFrequencies = new int[MAX_ALTITUDE + 1];

            //

            var altitudes = new int[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    float noiseVal = 0.0f;
                    for (var iOctave = 0; iOctave < noiseFrequencies.Length; iOctave++)
                    {
                        var xEffective = x;
                        var yEffective = y;

                        if (isWrapped)
                        {
                            //wrap around coordinates for a "spherical" world
                            xEffective = (x + width / 2) % width;
                            yEffective = (y + height / 2) % height;
                        }

                        var noiseX = noiseXOffset + noiseFrequencies[iOctave] * xEffective / width;
                        var noiseY = noiseYOffset + noiseFrequencies[iOctave] * yEffective / height;

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

            DebugUtils.Log($"DEBUG generateXxx - maxValue: {maxAltitude}, maxPerlin: {maxNoise}");
            for (var i = 0; i < countFrequencies.Length; i++)
            {
                DebugUtils.Log($"DEBUG - countFrequencies[{i}]: {countFrequencies[i]}");
            }

            DebugUtils.Log($"DEBUG - average noise: {cumNoise / (width * height)}");

            return altitudes;
        }


        private static int[,] generateLatitudes(int width, int height)
        {
            var latitudes = new int[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    latitudes[x, y] = Math.Round((2.0f * y / height - 1.0f) * MAX_LATITUDE);
                }
            }

            return latitudes;
        }

        private static int[,] computeTemperatures(int[,] altitudes, int[,] latitudes)
        {
            var temperatures = new int[altitudes.GetLength(0), altitudes.GetLength(1)];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    temperatures[x, y] = MAX_TEMPERATURE - Math.Floor(altitudes[x, y] - Math.Abs(latitudes[x, y]));
                }
            }

            return temperatures;
        }


        private static bool[,] generateLakes()
        {
            var lakes = new bool[altitudes.GetLength(0), altitudes.GetLength(1)];

            //TODO: generate lakes noise
            //highAltitudeLakeNoise = PerlinNoise(x * highAltitudeLakeScale, y * highAltitudeLakeScale)
            // maxHighAltitude = 0.8  # Altitudine massima consentita per laghi in altitudine elevata
            // if altitude > seaThreshold and altitude < maxHighAltitude and highAltitudeLakeNoise > highAltitudeLakeThreshold:
            //     terrainType = "lago"


            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    //lakes[x, y] = ...;
                }
            }

            return lakes;
        }


        private static bool[,] generateMoistures(bool[,] water)
        {
            var moisture = new int[water.GetLength(0), water.GetLength(1)];

            //TODO: generate independent, low frequency moisture noise

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    //moisture[x, y] = ...;
                }
            }

            return moisture;
        }


        private static TerrainType[,] computeBiomes(int[,] temperature, int[,] moisture)
        {
            const int M_0 = 3;
            const int M_1 = 4;
            const int M_2 = 5;
            const int M_3 = 6;
            const int M_4 = 9;

            const int T_0 = 3;
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

            var terrain = new int[water.GetLength(0), water.GetLength(1)];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var t = temperature[x, y];
                    var m = moisture[x, y];

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


        private static int rescaleAltitude(float noiseVal)
        {
            float[] altitudeLevels = new float[] { 0.72f, 0.8f, 0.85f, 0.9f };
            //float[] altitudeLevels = new float[] { 0.72f, 0.74f, 0.77f, 0.8f, 0.82f, 0.84f, 0.86f, 0.88f, 0.9f, 0.93f };
            Debug.Assert(altitudeLevels.Length == MAX_ALTITUDE);

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
