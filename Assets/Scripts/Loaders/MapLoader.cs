
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using Ventura.GameLogic;
using Ventura.Util;


namespace Ventura.Loaders
{
    public class MapLoader
    {
        private const string dataDir = "Data";

        private delegate Dictionary<string, T> ParseDataFile<T>(string[] jsonLines);


        private void LoadAllData()
        {
            var terrainTypes = new Dictionary<string, TerrainType>();
            var actorDefs = new Dictionary<string, Actor>();
            var itemDefs = new Dictionary<string, GameItem>();
            var mapDefs = new Dictionary<string, GameMap>();

            var actorFiles = new string[] { "monsters.json" };
            var itemFiles = new string[] { "items.json" };
            var mapFiles = new string[] { "test_map_world.txt", "test_map_milano.txt" };


            loadData<TerrainType>("terrains.txt", terrainTypes);
            
            foreach (var f in actorFiles)
                loadData<Actor>(f, actorDefs);

            foreach (var f in itemFiles)
                loadData<GameItem>(f, itemDefs);

            foreach (var f in mapFiles)
                loadData<GameMap>(f, mapDefs);
        }


        private static void loadData<T>(string filename, Dictionary<string, T> targetDict) where T : GameLogicObject
        {
            Messages.Log($"loadData({filename})");

            var jsonLines = File.ReadAllLines(filename);

            foreach (var jsonStr in jsonLines)
            {
                var loadedObj = JsonUtility.FromJson<T>(jsonStr);
                targetDict.Add(loadedObj.Name, loadedObj);
            }
        }

    }
}
