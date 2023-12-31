using TMPro;
using UnityEngine;
using Ventura.Graphics;
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
        private static StatusLineManager _instance;
        public static StatusLineManager Instance { get => _instance; }

        public TextMeshProUGUI statusLine;


        void Awake()
        {
            _instance = this;
        }

        public void Clear()
        {
            statusLine.text = "";
        }

        public void Display(string msg, StatusSeverity severity = StatusSeverity.Normal)
        {
            DebugUtils.Log("Display");
            DebugUtils.Log(msg);
            statusLine.color = GraphicsConfig.StatusLineColors[severity];
            statusLine.text = msg;

            UnityUtils.FlashAndFade(statusLine);
        }
    }
}
