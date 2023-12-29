using TMPro;
using UnityEngine;
using Ventura.Util;

namespace Ventura.Behaviours
{
    public enum StatusSeverity
    {
        Normal,
        Warning,
        Critical,
    }



    public class StatusLineManager : MonoBehaviour
    {
        private static StatusLineManager instance;
        private TextMeshProUGUI _statusLine;

        void Start()
        {
            instance = this;
            _statusLine = GameObject.Find("Status Line").GetComponent<TextMeshProUGUI>();
        }

        public static void DisplayStatus(string msg, StatusSeverity severity = StatusSeverity.Normal)
        {
            DebugUtils.Log("DisplayStatus");

            if (instance != null)
                instance.displayStatus(msg, severity);
        }

        private void displayStatus(string msg, StatusSeverity severity)
        {
            DebugUtils.Log("displayStatus");
            DebugUtils.Log(msg);
            _statusLine.color = GraphicsConfig.StatusLineColors[severity];
            _statusLine.text = msg;
        }
    }
}
