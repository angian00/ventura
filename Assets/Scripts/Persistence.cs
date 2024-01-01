using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Ventura.GameLogic;
using Ventura.Unity.Behaviours;
using Ventura.Util;

namespace Ventura
{

    public class Persistence
    {
        public static void SaveGame(string filename)
        {
            var fullPath = Application.persistentDataPath + "/" + filename;
            var orch = Orchestrator.Instance;
            
            DebugUtils.Log($"Saving game to {fullPath}");
            saveMap(orch.CurrMap, fullPath);
            DebugUtils.Log($"Game successfully saved");
        }

        public static void LoadGame(string filename)
        {
            var orch = Orchestrator.Instance;

            var fullPath = Application.persistentDataPath + "/" + filename;
            DebugUtils.Log($"Loading game from {fullPath}");
            
            //orch.ClearState();
            //orch.World = loadWorld();
            //orch.CurrMap = _allMaps[orch.CurrMapStack.CurrMapName];
            
            orch.CurrMap = loadMap(fullPath);
            orch.Player = orch.CurrMap.GetAnyEntity<Player>();

            //DEBUG
            //orch.CurrMap.DumpEntities();
            //

            ViewManager.Instance.Reset();
            PendingUpdates.Instance.AddAll();

            DebugUtils.Log($"Game successfully loaded");
        }

        private static void saveMap(GameMap gameMap, string path)
        {
            string jsonStr = JsonUtility.ToJson(gameMap);
            File.WriteAllText(path, jsonStr);
        }

        private static GameMap loadMap(string path)
        {
            var jsonStr = File.ReadAllText(path);
            return JsonUtility.FromJson<GameMap>(jsonStr);
        }
    }
}
