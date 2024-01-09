using UnityEditor;
using UnityEngine;

namespace Ventura.Unity.Editor
{
    public class TestPopupWindow : EditorWindow
    {
        private Rect _buttonRect;

        [MenuItem("Ventura/Tools/Test Popup")]
        public static void Init()
        {
            EditorWindow window = EditorWindow.CreateInstance<TestPopupWindow>();
            window.Show();
        }

        
        private void OnGUI()
        {
            GUILayout.Label("Ventura - TestPopup", EditorStyles.boldLabel);
            if (GUILayout.Button("Popup Options", GUILayout.Width(200)))
            {
                PopupWindow.Show(_buttonRect, new TestPopupContent());
            }

            if (Event.current.type == EventType.Repaint)
                buttonRect = GUILayoutUtility.GetLastRect();
        }
    }


    public class TestPopupContent : PopupWindowContent
    {
        bool toggle1 = true;
        bool toggle2 = true;
        bool toggle3 = true;

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 150);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("TestPopup Options", EditorStyles.boldLabel);
            toggle1 = EditorGUILayout.Toggle("Toggle 1", toggle1);
            toggle2 = EditorGUILayout.Toggle("Toggle 2", toggle2);
            toggle3 = EditorGUILayout.Toggle("Toggle 3", toggle3);
        }

        public override void OnOpen()
        {
            Debug.Log("Popup opened: " + this);
        }

        public override void OnClose()
        {
            Debug.Log("Popup closed: " + this);
        }
    }
}
