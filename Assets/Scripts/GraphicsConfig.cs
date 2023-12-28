using UnityEngine;
using System.Collections.Generic;
using Ventura.GameLogic;


public static class GraphicsConfig
{
    public static readonly Dictionary<string, string> EntityIcons = new()
        {
            { "site", "temple" },
        };


    public static readonly Dictionary<TerrainType, Color> TerrainColors  = new ()
        {
            { TerrainType.Plains1, fromHex("#A5A58D") },
            { TerrainType.Plains2, fromHex("#B7B7A4") },
            { TerrainType.Hills1, fromHex("#6B705C") },
            { TerrainType.Hills2, fromHex("#CB997E") },
            { TerrainType.Mountains, fromHex("#DDBEA9") },
        };


    private static Color fromHex(string hexStr)
    {
        Color res;
        if (ColorUtility.TryParseHtmlString(hexStr, out res))
            return res;

        Debug.LogError($"!! Invalid color hex string: {hexStr}");
        return Color.red;
    }
}
