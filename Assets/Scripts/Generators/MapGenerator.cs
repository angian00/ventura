using UnityEngine;
using Random = UnityEngine.Random;
using System;
using Ventura.GameLogic;
using Ventura.Util;


namespace Ventura.Generators
{

    public class MapGenerator
    {
        public static GameMap GenerateWildernessMap(int nRows, int nCols, string? mapName=null, bool hasSites=true)
        {
            Messages.Log("GenerateWildernessMap()");

            // TODO: use Perlin noise
            const double percForest = 0.1;

            if (mapName == null)
                mapName = NameGenerator.GenerateMapName();

            var newMap = new GameMap(mapName, mapName, nRows, nCols);
            
            int nForest = (int)Math.Round(percForest * nRows * nCols);

            for (int i = 0; i < nForest; i++)
            {
                int x = Random.Range(0, nRows);
                int y = Random.Range(0, nCols);

                //TODO: repeat if it is already a Mountain
                newMap.Terrain[x, y] = TerrainType.Mountain;
            }

            if (hasSites)
                addSites(newMap, 8);

            //choose starting pos on a empty square
            while (true)
            {
                int x = Random.Range(0, nRows);
                int y = Random.Range(0, nCols);
                if (newMap.Terrain[x, y].Walkable && (newMap.GetEntitiesAt(x, y).Count == 0))
                {
                    newMap.StartingPos = new Vector2Int(x, y);
                    break;
                }
            }

            return newMap;
        }


		//TODO: differentiate by site type
		private static void addSites(GameMap targetMap, int nSites)
		{
            Messages.Log($"addSite({targetMap.Name}, {nSites})");

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
                    if ((e.X == x && e.Y == y) || (e is Site && Math.Abs(e.X - x) < MIN_SITE_DISTANCE && Math.Abs(e.Y - y) < MIN_SITE_DISTANCE))
                    {
                        isTileOk = false;
                        break;
                    }
                }

                if (!isTileOk)
                    continue;


                var siteName = NameGenerator.GenerateMapName();
                var newSite = new Site(siteName, targetMap.Name);
                newSite.MoveTo(x, y);

                targetMap.Entities.Add(newSite);

                i++;
            }
		}
    }
}