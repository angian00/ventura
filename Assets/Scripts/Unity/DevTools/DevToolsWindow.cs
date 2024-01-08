#if (UNITY_EDITOR) 
using UnityEditor;
using UnityEngine;

namespace Ventura.Unity.DevTools
{

    public class DevToolsWindow : EditorWindow
    {
        [MenuItem("Window/AnGian DevTools")]
        public static void ShowWindow()
        {
            GetWindow<DevToolsWindow>("AnGian DevTools");
        }


        private void OnGUI()
        {
            GUILayout.Label("ScriptableObject Instances:");

            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (scriptableObject != null && IsMine(scriptableObject))
                {
                    EditorGUILayout.LabelField(scriptableObject.name);
                }
            }
        }

        private static bool IsMine(ScriptableObject sObj)
        {
            return sObj.GetType().Namespace.Contains("Ventura");
        }
    }
}
#endif
