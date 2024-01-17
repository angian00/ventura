using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ventura.Test.WorldGenerating;
using Ventura.Util;

namespace Ventura.Test
{
    public class TestWorldGenerator : MonoBehaviour
    {
        public GameObject terrainTileTemplate;
        public Transform terrainLayer;
        public TMP_Dropdown mapTypeDropdown;


        private static int MAP_WIDTH = 40;
        private static int MAP_HEIGHT = 30;

        private Dictionary<TerrainType, Color> biomeColors;

        private GameObject[,] _mapTiles;
        private GameMap _world;


        private enum MapType
        {
            Altitude,
            Latitude,
            Temperature,
            Moisture,
            Terrain,
        }



        private void Awake()
        {
            initColors();
            initMap();

            mapTypeDropdown.onValueChanged.AddListener(delegate { onMapTypeChanged(); });
            mapTypeDropdown.ClearOptions();
            mapTypeDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(MapType))));
            mapTypeDropdown.RefreshShownValue();

            mapTypeDropdown.value = -1;
        }


        public void OnButtonClicked()
        {
            _world = WorldGenerator.GenerateWorld(MAP_WIDTH, MAP_HEIGHT);
            onMapTypeChanged();
        }

        private void onMapTypeChanged()
        {
            if (_world == null)
                return;

            var mapType = Enum.Parse<MapType>(mapTypeDropdown.options[mapTypeDropdown.value].text);
            DebugUtils.Log($"onMapTypeChanged: {mapType}");
            updateView(_world, mapType);
        }


        private void initColors()
        {
            biomeColors = new Dictionary<TerrainType, Color>
            {
                {TerrainType.Water, (Color)UnityUtils.ColorFromHex("#015482") },
                {TerrainType.Desert, (Color)UnityUtils.ColorFromHex("#EDC9AF") },
                {TerrainType.Grass, (Color)UnityUtils.ColorFromHex("#ACD48F")},
                {TerrainType.Forest, (Color)UnityUtils.ColorFromHex("#1C3E0B") },
                {TerrainType.Tropical, (Color)UnityUtils.ColorFromHex("#85d000") },
                {TerrainType.Rock, (Color)UnityUtils.ColorFromHex("#745B55") },
                {TerrainType.Snow, (Color)UnityUtils.ColorFromHex("#F7F9F7") },
            };
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


        private void updateView(GameMap world, MapType mapType)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    Color tileColor = Color.magenta;
                    switch (mapType)
                    {
                        case MapType.Altitude:
                            tileColor = valueColor(world.altitudes[x, y]);
                            break;
                        case MapType.Latitude:
                            tileColor = valueColor(Math.Abs(world.latitudes[x, y]));
                            break;
                        case MapType.Temperature:
                            tileColor = valueColor(world.temperatures[x, y]);
                            break;
                        case MapType.Moisture:
                            tileColor = valueColor(world.moistures[x, y]);
                            break;
                        case MapType.Terrain:
                            tileColor = biomeColors[world.terrain[x, y]];
                            break;
                    }

                    _mapTiles[x, y].GetComponent<SpriteRenderer>().color = tileColor;
                }
            }
        }

        private static Color valueColor(int value, int maxValue = 10)
        {
            if (value == 0)
                return Color.blue;

            Color baseColor = Color.red;

            return Color.Lerp(Color.black, baseColor, (float)value / maxValue);
        }
    }
}
