using UnityEditor;
using UnityEngine;

namespace Ventura.Unity.Editor
{
    public class BackupSOWindow : EditorWindow
    {

        [MenuItem("Ventura/Tools/Backup ScriptableObject instances")]
        public static void ShowWindow()
        {
            GetWindow<DevToolsWindow>("Backing up ScriptableObject instances");
        }


        private void OnGUI()
        {
            var soObjs = getVenturaScriptableObjs();
            
            var rootBackupPath = $"{Application.dataPath}/Backups";
            var dateStr =  DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
            var folderPath = $"{rootBackupPath}/backup_{dateStr}";

            foreach (var sObj in soObjs)
            {
                var fullPath = $"{folderPath}/{sObj.name}.json";
                
                var jsonStr = JsonUtility.ToJson(sObj);
                File.WriteAllText(fullPath, jsonStr);

                printMessage($"[{sObj.name}] saved to [{fullPath}]");
            }
        }

        private List<ScriptableObject> getVenturaScriptableObjs()
        {
            var res = new List<ScriptableObject>();

            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (scriptableObject != null && isMine(scriptableObject))
                {
                    res.Add(scriptableObject);
                }
            }

            return res;
        }


        private static bool isMine(ScriptableObject sObj)
        {
            return sObj.GetType().Namespace.Contains("Ventura");
        }

        private void printMessage(string message)
        {
            GUILayout.Label(message);
        }
    }
}
