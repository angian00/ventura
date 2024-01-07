using TMPro;
using UnityEngine;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class StatusLineBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI statusLine;

        private void OnEnable()
        {
            EventManager.Subscribe<TextNotification>(onTextNotification);
            //EventManager.Subscribe(onSystemActionTest);

        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<TextNotification>(onTextNotification);
        }


        public void onTextNotification(TextNotification eventData)
        {
            DebugUtils.Warning($"DEBUG: StatusLineBehaviour.onTextNotification; msg is [{eventData.msg}]");
            var msg = eventData.msg;
            if (msg != null && msg != "")
                DebugUtils.Log(msg);

            //statusLine.color = GraphicsConfig.StatusLineColors[severity]; //FIXME
            statusLine.text = msg;

            UnityUtils.FlashAndFade(statusLine);
        }
    }
}
