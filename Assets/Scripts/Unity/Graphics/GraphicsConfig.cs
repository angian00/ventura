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
        };


        public static readonly Dictionary<TerrainType, Color> TerrainColors = new()
        {
            { TerrainType.Plains1, FromHex("#A5A58D") },
            { TerrainType.Plains2, FromHex("#B7B7A4") },
            { TerrainType.Hills1, FromHex("#6B705C") },
            { TerrainType.Hills2, FromHex("#CB997E") },
            { TerrainType.Mountains, FromHex("#DDBEA9") },
        };

        //darkTile: TerrainAspect
        //lightTile: TerrainAspect



        public static readonly Dictionary<StatusSeverity, Color> StatusLineColors = new()
        {
            { StatusSeverity.Normal, Color.white },
            { StatusSeverity.Warning, FromHex("#FFA500") },
            { StatusSeverity.Critical, Color.red },
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