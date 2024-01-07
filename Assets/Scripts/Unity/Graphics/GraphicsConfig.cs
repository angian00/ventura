using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Unity.Events;

namespace Ventura.Unity.Graphics
{

    public class GraphicsConfig
    {
        public static readonly Dictionary<string, string> SpriteFiles = new()
        {
            { "site", "temple" },
            { "item", "powder-bag" },

            { "book", "book_00" },

            { "butterfly", "butterfly" },
        };


        public static readonly Dictionary<TerrainType, Color> TerrainColors = new()
        {
            { TerrainType.Water, FromHex("#1CA3EC") },
            { TerrainType.Plains1, FromHex("#A5A58D") },
            { TerrainType.Plains2, FromHex("#B7B7A4") },
            { TerrainType.Hills1, FromHex("#6B705C") },
            { TerrainType.Hills2, FromHex("#CB997E") },
            { TerrainType.Mountains, FromHex("#DDBEA9") },
        };

        //darkTile: TerrainAspect
        //lightTile: TerrainAspect



        public static readonly Dictionary<TextNotification.Severity, Color> StatusLineColors = new()
        {
            { TextNotification.Severity.Normal, Color.white },
            { TextNotification.Severity.Warning, FromHex("#FFA500") },
            { TextNotification.Severity.Critical, Color.red },
        };



        public static Color FromHex(string hexStr)
        {
            Color res;
            if (ColorUtility.TryParseHtmlString(hexStr, out res))
                return res;

            Debug.LogError($"!! Invalid color hex string: {hexStr}");
            return Color.red;
        }
    }
}