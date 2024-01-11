using TMPro;
using UnityEngine;
using Ventura.Unity.Events;
using Ventura.Unity.ScriptableObjects;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class StatusLineBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI statusLine;
        public SeverityColorConfig severityColors;


        private void OnEnable()
        {
            EventManager.Subscribe<TextNotification>(onTextNotification);

        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<TextNotification>(onTextNotification);
        }


        public void onTextNotification(TextNotification eventData)
        {
            //DebugUtils.Warning($"DEBUG: StatusLineBehaviour.onTextNotification; msg is [{eventData.msg}]");
            var msg = eventData.msg;
            if (msg != null && msg != "")
                DebugUtils.Log(msg);

            statusLine.color = severityColors.Get(eventData.severity);
            statusLine.text = msg;

            UnityUtils.FlashAndFade(statusLine);
        }
    }
}
