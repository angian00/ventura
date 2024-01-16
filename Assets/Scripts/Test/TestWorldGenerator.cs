using System.Collections.Generic;
using UnityEngine;
using Ventura.Test.WorldGenerating;

namespace Ventura.Test
{


    public class TestWorldGenerator : MonoBehaviour
    {
        private static Color visibleColor = Color.green;
        private static Color notVisibleColor = Color.blue;
        private static Color startColor = Color.red;
        private static Color endColor = Color.yellow;
        private static Color wallColor = Color.grey;

        public GameObject terrainTileTemplate;
        public Transform terrainLayer;

        private GameObject[,] _mapTiles;

        private static int MAP_WIDTH = 42;
        private static int MAP_HEIGHT = 30;

        private static Dictionary<int, Color> tileColors;


        static TestWorldGenerator()
        {
            tileColors = new Dictionary<int, Color>
            {
                {0, Color.blue },
                {1, Color.green },
                {2, Color.yellow },
                {3, Color.red },
                {4, Color.white },
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
                    newMapTile.GetComponent<SpriteRenderer>().color = notVisibleColor;
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
                    Color tileColor = tileColors[world.altitudes[x, y]];

                    _mapTiles[x, y].GetComponent<SpriteRenderer>().color = tileColor;
                }
            }
        }
    }
}
