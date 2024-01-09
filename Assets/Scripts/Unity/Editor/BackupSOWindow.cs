using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Ventura.Util;

namespace Ventura.Unity.Editor
{
    public class BackupSOWindow
    {

        [MenuItem("Ventura/Tools/Backup ScriptableObjects")]
        public static void DoBackup()
        {
            DebugUtils.Log($"Backing up Ventura ScriptableObject instances");

            var soObjs = getVenturaScriptableObjs();

            var rootBackupPath = $"{Application.dataPath}/Backups";
            if (!Directory.Exists(rootBackupPath))
                Directory.CreateDirectory(rootBackupPath);

            var dateStr = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
            var folderPath = $"{rootBackupPath}/backup_{dateStr}";

            //rewrite old backup if less than 1 minute old
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);

            Directory.CreateDirectory(folderPath);

            foreach (var sObj in soObjs)
            {
                var fullPath = $"{folderPath}/{sObj.name}.json";

                var jsonStr = JsonUtility.ToJson(sObj);
                File.WriteAllText(fullPath, jsonStr);

                DebugUtils.Log($"[{sObj.name}] saved to [{fullPath}]");

            }
        }


        private static List<ScriptableObject> getVenturaScriptableObjs()
        {
            var res = new List<ScriptableObject>();

            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject sObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (sObj != null && sObj.GetType().Namespace.Contains("Ventura"))
                {
                    DebugUtils.Log($"Found ScriptableObject [{sObj.name}] [{guid}]");

                    res.Add(sObj);
                }
            }

            return res;
        }
    }
}
