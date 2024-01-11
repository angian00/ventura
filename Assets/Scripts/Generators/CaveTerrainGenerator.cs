using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Generators
{

    public class CaveTerrainGenerator
    {
        /**
         * Using cellular automaton algorithm
         */
        public static TerrainDef[,] generateTerrain(int width, int height)
        {
            const float startWallPerc = 0.55f;
            const int deathThreshold = 4;
            const int birthThreshold = 5;
            const int nGenerations = 6;

            TerrainDef[,] terrainMap = new TerrainDef[width, height];


            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    TerrainDef terrain;

                    if (Random.value < startWallPerc)
                        terrain = TerrainDef.Wall;
                    else
                        terrain = TerrainDef.Room;

                    terrainMap[x, y] = terrain;
                }
            }

            for (int iGeneration = 0; iGeneration < nGenerations; iGeneration++)
            {
                var oldTerrainMap = terrainMap.Clone() as TerrainDef[,];

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var nNeighbours = DataUtils.CountMatchingNeighbours(oldTerrainMap, TerrainDef.Wall, x, y, 1);

                        TerrainDef newTerrain;

                        if (oldTerrainMap[x, y] == TerrainDef.Wall)
                        {
                            if (nNeighbours <= deathThreshold)
                                newTerrain = TerrainDef.Room;
                            else
                                newTerrain = TerrainDef.Wall;

                        }
                        else
                        {
                            if (nNeighbours >= birthThreshold)
                                newTerrain = TerrainDef.Wall;
                            else
                                newTerrain = TerrainDef.Room;
                        }

                        terrainMap[x, y] = newTerrain;
                    }
                }
            }

            return terrainMap;
        }



    }
}