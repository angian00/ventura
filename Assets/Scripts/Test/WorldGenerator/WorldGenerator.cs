using System;
using TinkerWorX.AccidentalNoiseLibrary;
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
        public float[,] altitudes;
        public float[,] temperatures;
        public float[,] moistures;
        public TerrainType[,] terrain;


        public GameMap(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }


    public class WorldGenerator
    {
        private const float WATER_ALTITUDE = 0.1f;

        public int width;
        public int height;
        public AnimationCurve altitudeMapping;
        public int nAltitudeOctaves;
        public AnimationCurve moistureMapping;
        public int nMoistureOctaves;
        public AnimationCurve temperatureMapping;
        public int nTemperatureOctaves;


        public WorldGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public GameMap GenerateWorld()
        {
            var t0 = Time.realtimeSinceStartup;
            var gameMap = new GameMap(width, height);
            var tMap = Time.realtimeSinceStartup;
            DebugUtils.Log($"GenerateWorld; tMap duration : {(tMap - t0):f2} seconds");

            gameMap.altitudes = generateAltitudes();
            var tAlt = Time.realtimeSinceStartup;
            DebugUtils.Log($"GenerateWorld; tAlt duration : {(tAlt - tMap):f2} seconds");

            gameMap.temperatures = generateTemperatures(gameMap.altitudes);
            var tTemp = Time.realtimeSinceStartup;
            DebugUtils.Log($"GenerateWorld; tTemp duration : {(tTemp - tAlt):f2} seconds");

            gameMap.moistures = generateMoistures(gameMap.altitudes);
            var tMoist = Time.realtimeSinceStartup;
            DebugUtils.Log($"GenerateWorld; tMoist duration : {(tMoist - tTemp):f2} seconds");

            gameMap.terrain = computeBiomes(gameMap.temperatures, gameMap.moistures);
            var tTerrain = Time.realtimeSinceStartup;
            DebugUtils.Log($"GenerateWorld; tTerrain duration : {(tTerrain - tMoist):f2} seconds");

            return gameMap;
        }

        private float[,] generateAltitudes()
        {
            DebugUtils.Log("generateAltitudes");

            return generateNoise(width, height, true, altitudeMapping, nAltitudeOctaves);
        }


        private float[,] generateTemperatures(float[,] altitudes)
        {
            var altWeight = 1.0f;
            var latWeight = 2.0f;
            var noiseWeight = 1.0f;

            var w = altitudes.GetLength(0);
            var h = altitudes.GetLength(1);
            var temperatures = new float[w, h];

            var noiseValues = generateNoise(w, h, true, temperatureMapping, nTemperatureOctaves);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var latitude = 2.0f * y / h - 1.0f;

                    //water mitigates temperature extremes
                    var valueVariance = 1.0f;
                    if (altitudes[x, y] <= WATER_ALTITUDE)
                        valueVariance = 0.4f;

                    var altValue = 1.0f - altitudes[x, y];
                    var latValue = 1.0f - Math.Abs(latitude);
                    var noiseValue = noiseValues[x, y];

                    temperatures[x, y] = (1 - valueVariance) * 0.5f +
                        valueVariance * (altWeight * altValue + latWeight * latValue + noiseWeight * noiseValue) / (altWeight + latWeight + noiseWeight);
                }
            }

            return temperatures;
        }


        private float[,] generateMoistures(float[,] altitudes)
        {
            DebugUtils.Log("generateMoistures");

            var baseWeight = 1.0f;
            var latWeight = 0.5f;
            var noiseWeight = 1.0f;

            var w = altitudes.GetLength(0);
            var h = altitudes.GetLength(1);
            var moistures = new float[w, h];

            var seaMoistures = computeSeaMoistureFloat(altitudes);
            var noiseValues = generateNoise(w, h, true, moistureMapping, nMoistureOctaves);

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    if (seaMoistures[x, y] > 0.95f)
                        moistures[x, y] = seaMoistures[x, y];
                    else
                    {
                        var latitude = 2.0f * y / h - 1.0f;

                        var baseValue = 0.3f + seaMoistures[x, y];
                        var latValue = 1.0f - Math.Abs(latitude);
                        var noiseValue = noiseValues[x, y];

                        moistures[x, y] = (baseWeight * baseValue + latWeight * latValue + noiseWeight * noiseValue) / (baseWeight + latWeight + noiseWeight);
                    }
                }
            }

            return moistures;
        }


        private TerrainType[,] computeBiomes(float[,] temperatures, float[,] moistures)
        {
            const float M_0 = .3f;
            const float M_1 = .4f;
            const float M_2 = .5f;
            const float M_3 = .6f;
            const float M_4 = .9f;

            const float T_0 = .4f;
            const float T_1 = .5f;
            const float T_2 = .7f;

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

        private static float[,] generateNoise(int width, int height, bool isWrapped, AnimationCurve noiseEqualizer, int nOctaves)
        {
            ImplicitFractal HeightMap = new ImplicitFractal(FractalType.Multi, BasisType.Simplex, InterpolationType.Quintic);
            HeightMap.Octaves = nOctaves;
            HeightMap.Lacunarity = 2.5;
            //HeightMap.Lacunarity = 1.2;

            var minNoise = 999.0f;
            var maxNoise = -999.0f;
            var cumNoise = 0.0f;

            var noiseValues = new float[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var xNormalized = ((float)x) / width;
                    var yNormalized = ((float)y) / height;

                    float noiseValue;
                    if (isWrapped)
                    {
                        var s = xNormalized;
                        var nx = Math.Cos(s * 2 * MathF.PI) / (2 * Mathf.PI);
                        var ny = Math.Sin(s * 2 * MathF.PI) / (2 * Mathf.PI);
                        var nz = yNormalized;

                        noiseValue = (float)HeightMap.Get(nx, ny, nz);
                    }
                    else
                    {
                        noiseValue = (float)HeightMap.Get(xNormalized, yNormalized);
                    }


                    noiseValues[x, y] = noiseValue;
                    //DebugUtils.Log($"noiseValue ({x}, {y}): {noiseValue}");

                    if (noiseValue > maxNoise)
                        maxNoise = noiseValue;
                    if (noiseValue < minNoise)
                        minNoise = noiseValue;
                    cumNoise += noiseValue;
                }
            }

            DebugUtils.Log($"DEBUG generateNoise - min noise: {minNoise}, max noise: {maxNoise}, avg noise: {cumNoise / (width * height)}");

            var res = new float[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    res[x, y] = noiseEqualizer.Evaluate(rescaleNoise(noiseValues[x, y], minNoise, maxNoise));
                }
            }

            var noiseHistogram = DataUtils.ComputeHistogram(res);
            for (var i = 0; i < noiseHistogram.Length; i++)
            {
                DebugUtils.Log($"DEBUG - noise distribution [{((float)i) / noiseHistogram.Length}]: {noiseHistogram[i]}");
            }

            return res;
        }


        private float[,] computeSeaMoistureFloat(float[,] altitudes)
        {
            const float coastRelativeSize = 0.04f;
            const float maxCoastMoisture = 0.3f;

            var coastTileSize = coastRelativeSize * (width + height) / 2;
            var coastTileSize2 = coastTileSize * coastTileSize;

            var res = new float[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (altitudes[x, y] > WATER_ALTITUDE)
                        continue;

                    res[x, y] = 1.0f;

                    var x1Start = Math.Max(0, (int)Math.Floor(x - coastTileSize));
                    var x1End = Math.Min(width, (int)Math.Ceiling(x + coastTileSize));
                    var y1Start = Math.Max(0, (int)Math.Floor(y - coastTileSize));
                    var y1End = Math.Min(height, (int)Math.Ceiling(y + coastTileSize));

                    for (var x1 = x1Start; x1 < x1End; x1++)
                    {
                        var dx2 = (x - x1) * (x - x1);

                        for (var y1 = y1Start; y1 < y1End; y1++)
                        {
                            var dy2 = (y - y1) * (y - y1);

                            if (dx2 + dy2 < coastTileSize2)
                            {
                                var coastMoisture = maxCoastMoisture * (1.0f - (dx2 + dy2) / coastTileSize2);
                                if (coastMoisture > res[x1, y1])
                                    res[x1, y1] = coastMoisture;
                            }
                        }
                    }
                }
            }

            return res;
        }


        private static float rescaleNoise(float noiseValue, float minValue, float maxValue)
        {
            //renormalize from 0 to 1
            return (noiseValue - minValue) / (maxValue - minValue);
        }
    }
}
