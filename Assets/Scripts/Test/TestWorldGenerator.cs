using System.Collections.Generic;
using UnityEngine;
using Ventura.Test.WorldGenerating;


namespace Ventura.Test
{
    public class TestWorldGenerator : MonoBehaviour
    {
        public GameObject terrainTileTemplate;
        public Transform terrainLayer;

        private GameObject[,] _mapTiles;

        private static int MAP_WIDTH = 40;
        private static int MAP_HEIGHT = 30;

        private static Dictionary<int, Color> biomeColors;


        static TestWorldGenerator()
        {
            biomeColors = new Dictionary<TerrainType, Color>
            {
                {TerrainType.Water, Color.blue },
                {TerrainType.Desert, Color.red },
                {TerrainType.Grass, Color.green },
                {TerrainType.Forest, Color.black }, //TODO
                {TerrainType.Tropical, Color.yellow },
                {TerrainType.Rock, Color.grey },
                {TerrainType.Snow, Color.white },
            };
        }



        private void Awake()
        {
            initMap();
        }

        public void OnButtonClicked()
        {
            var world = WorldGenerator.GenerateWorld(MAP_WIDTH, MAP_HEIGHT);
            updateView(world);
        }

        private void initMap()
        {
            _mapTiles = new GameObject[MAP_WIDTH, MAP_HEIGHT];
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newMapTile.GetComponent<SpriteRenderer>().color = Color.magenta;
                    newMapTile.transform.SetParent(terrainLayer);
                    _mapTiles[x, y] = newMapTile;
                }
            }
        }


        private void updateView(GameMap world)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    Color tileColor = valueColor(world.altitudes[x, y]);
                    //Color tileColor = valueColor(Math.abs(world.latitudes[x, y]));
                    //Color tileColor = valueColor(world.temperatures[x, y]);
                    //Color tileColor = valueColor(world.moistures[x, y]);
                    //Color tileColor = biomeColors[world.terrain[x, y]];

                    _mapTiles[x, y].GetComponent<SpriteRenderer>().color = tileColor;
                }
            }
        }

        private static Color valueColor(int value, int maxValue = 10)
        {
            const Color baseColor = Color.red;

            return Color.lerp(baseColor.black, baseColor, (float)value / maxValue);
        }
    }
}
