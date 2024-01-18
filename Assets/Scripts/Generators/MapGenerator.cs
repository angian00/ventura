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
        public enum MapType
        {
            Cave,
            Wilderness,
        }

        public static GameMap GenerateMap(MapType mapType, string? mapName = null)
        {
            DebugUtils.Log("GenerateMap()");
            const int WORLD_MAP_WIDTH = 80;
            const int WORLD_MAP_HEIGHT = 65;

            if (mapName == null)
                mapName = FileStringGenerator.Sites.GenerateString();


            TerrainDef[,] terrainMap;
            switch (mapType)
            {
                case MapType.Wilderness:
                    terrainMap = WildernessTerrainGenerator.generateTerrain(WORLD_MAP_WIDTH, WORLD_MAP_HEIGHT);
                    break;
                case MapType.Cave:
                    terrainMap = CaveTerrainGenerator.generateTerrain(20, 20);
                    break;
                default:
                    throw new GameException($"Invalid map type: {DataUtils.EnumToStr(mapType)}");
            }

            var newMap = new GameMap(mapName, mapName, terrainMap);

            if (mapType == MapType.Wilderness)
                generateSites(newMap);
            //generateSites(newMap, 1);

            generateSomeItems(newMap);
            generateSomeMonsters(newMap);
            //generateSomeItems(newMap, 1);
            //generateSomeMonsters(newMap, 1);

            newMap.StartingPos = RandomUtils.RandomEmptyPos(newMap);

            return newMap;
        }


        private static void generateSites(GameMap targetMap, int nSites = -1)
        {
            const int MIN_SITE_DISTANCE = 3;

            //DebugUtils.Log($"generateSites({targetMap.Name}, {nSites})");
            const float perc = .004f;

            if (nSites == -1)
                nSites = (int)(perc * targetMap.Width * targetMap.Height);


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