using System.Collections.Generic;
using UnityEngine;
using Ventura.Unity.Events;

namespace Ventura.Unity.Graphics
{

    public class GraphicsConfigOld
    {
        public static readonly Dictionary<string, string> SpriteFiles = new()
        {
            { "site", "temple" },
            { "item", "powder-bag" },
            { "player", "character" },

            { "book", "book_00" },

            { "butterfly", "butterfly" },
        };


        //public static readonly Dictionary<TerrainDef, Color> TerrainColors = new()
        //{
        //    { TerrainDef.Water, FromHex("#1CA3EC") },
        //    { TerrainDef.Plains1, FromHex("#A5A58D") },
        //    { TerrainDef.Plains2, FromHex("#B7B7A4") },
        //    { TerrainDef.Hills1, FromHex("#6B705C") },
        //    { TerrainDef.Hills2, FromHex("#CB997E") },
        //    { TerrainDef.Mountains, FromHex("#DDBEA9") },
        //};



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