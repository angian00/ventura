
using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Unity.Config
{
    [Serializable]
    public record EnumColorConfig<T> where T : Enum
    {
        public T enumValue;
        public Color color;
    }

    [CreateAssetMenu]
    public class GameGraphicsConfig : ScriptableObject
    {
        private class EnumColorMap<T> where T : Enum
        {
            private Dictionary<T, Color> _data = new();

            public Color Get(T key) => _data[key];
            public void Add(T key, Color color) => _data[key] = color;
        }

        //public static readonly Dictionary<string, string> SpriteFiles = new()
        //{
        //    { "site", "temple" },
        //    { "item", "powder-bag" },
        //    { "player", "character" },

        //    { "book", "book_00" },

        //    { "butterfly", "butterfly" },
        //};


        public List<EnumColorConfig<TerrainDef.TerrainType>> terrainColorConfigs;

        public Color GetColor(TerrainDef.TerrainType terrainType) => _terrainColorMap.Get(terrainType);


        private EnumColorMap<TerrainDef.TerrainType> _terrainColorMap;

        //public static readonly Dictionary<TextNotification.Severity, Color> StatusLineColors = new()
        //{
        //    { TextNotification.Severity.Normal, Color.white },
        //    { TextNotification.Severity.Warning, FromHex("#FFA500") },
        //    { TextNotification.Severity.Critical, Color.red },
        //};


        void OnEnable()
        {
            DebugUtils.Log($"Reconstructing GameGraphicsConfig [{this.name}]");

            _terrainColorMap = new();

            foreach (var terrainColorConfig in terrainColorConfigs)
                _terrainColorMap.Add(terrainColorConfig.enumValue, terrainColorConfig.color);
        }

    }
}