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
        public static StatusLineManager Instance { get { return instance; } }

        private TextMeshProUGUI _statusLine;

        void Start()
        {
            instance = this;
            _statusLine = GameObject.Find("Status Line").GetComponent<TextMeshProUGUI>();
        }

        public void Clear()
        {
            _statusLine.text = "";
        }

        public void Display(string msg, StatusSeverity severity = StatusSeverity.Normal)
        {
            DebugUtils.Log("Display");
            DebugUtils.Log(msg);
            _statusLine.color = GraphicsConfig.StatusLineColors[severity];
            _statusLine.text = msg;
        }
    }
}
