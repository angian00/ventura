using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Test.WorldGenerating;
using Ventura.Util;

namespace Ventura.Test
{
    public class TestWorldGenerator : MonoBehaviour
    {
        private enum MapType
        {
            Altitude,
            Temperature,
            Moisture,
            Terrain,
        }

        public TMP_Dropdown mapTypeDropdown;
        public Image mapImage;

        //private static int mapWidth = 40;
        //private static int mapHeight = 30;
        public int mapWidth = 40 * 10;
        public int mapHeight = 30 * 10;

        public AnimationCurve altitudeMapping = AnimationCurve.Linear(0, 0, 1, 1);
        public int nAltitudeOctaves = 5;

        public AnimationCurve moistureMapping = AnimationCurve.Linear(0, 0, 1, 1);
        public int nMoistureOctaves = 3;

        public AnimationCurve temperatureMapping = AnimationCurve.Linear(0, 0, 1, 1);
        public int nTemperatureOctaves = 5;

        private Dictionary<TerrainType, Color> biomeColors;
        private GameMap _world;



        private void Awake()
        {
            initColors();

            mapTypeDropdown.onValueChanged.AddListener(delegate { onMapTypeChanged(); });
            mapTypeDropdown.ClearOptions();
            mapTypeDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(MapType))));
            mapTypeDropdown.RefreshShownValue();

            mapTypeDropdown.value = -1;
        }


        public void OnButtonClicked()
        {
            DebugUtils.Log("OnButtonClicked");

            var t0 = Time.realtimeSinceStartup;

            var worldGen = new WorldGenerator(mapWidth, mapHeight);
            worldGen.altitudeMapping = altitudeMapping;
            worldGen.nAltitudeOctaves = nAltitudeOctaves;
            worldGen.temperatureMapping = temperatureMapping;
            worldGen.nTemperatureOctaves = nTemperatureOctaves;
            worldGen.moistureMapping = moistureMapping;
            worldGen.nMoistureOctaves = nMoistureOctaves;

            _world = worldGen.GenerateWorld();

            var t1 = Time.realtimeSinceStartup;
            DebugUtils.Log($"GenerateWorld duration : {(t1 - t0):f2} seconds");
            onMapTypeChanged();
            var t2 = Time.realtimeSinceStartup;
            DebugUtils.Log($"onMapTypeChanged duration : {(t2 - t1):f2} seconds");
        }

        private void onMapTypeChanged()
        {
            if (_world == null)
                return;

            var mapType = Enum.Parse<MapType>(mapTypeDropdown.options[mapTypeDropdown.value].text);
            DebugUtils.Log($"onMapTypeChanged: {mapType}");

            mapImage.sprite = createMapSprite(_world, mapType);
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


        private Sprite createMapSprite(GameMap world, MapType mapType)
        {
            var texture = new Texture2D(world.width, world.height, TextureFormat.ARGB32, false);

            for (int x = 0; x < world.width; x++)
            {
                for (int y = 0; y < world.height; y++)
                {
                    Color tileColor = Color.magenta;
                    switch (mapType)
                    {
                        case MapType.Altitude:
                            tileColor = valueColor(world.altitudes[x, y]);
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

                    texture.SetPixel(x, y, tileColor);
                }
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(texture.width / 2, texture.height / 2));
        }


        private static Color valueColor(float value)
        {
            float EPSILON = 0.1f;

            if (value < EPSILON)
                return Color.blue;

            Color baseColor = Color.red;

            return Color.Lerp(Color.black, baseColor, value);
        }
    }
}
