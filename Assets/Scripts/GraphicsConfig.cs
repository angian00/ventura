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
            { TerrainType.Plains, fromHex("#87b6a7") },
            //{ TerrainType.Forest, fromHex("#315c2b") },
            { TerrainType.Mountain, fromHex("#4F3130") },
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
