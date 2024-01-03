using TMPro;
using UnityEngine;
using Ventura.Unity.Events;
using Ventura.Unity.Graphics;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class StatusLineManager : MonoBehaviour
    {
        public TextMeshProUGUI statusLine;

        private void OnEnable()
        {
            EventManager.StatusNotificationEvent.AddListener(onStatusNotification);
        }

        private void OnDisable()
        {
            EventManager.StatusNotificationEvent.RemoveListener(onStatusNotification);
        }


        public void Clear()
        {
            statusLine.text = "";
        }

        private void onStatusNotification(string msg, StatusSeverity severity)
        {
            DebugUtils.Log(msg);
            statusLine.color = GraphicsConfig.StatusLineColors[severity];
            statusLine.text = msg;

            UnityUtils.FlashAndFade(statusLine);
        }
    }
}
