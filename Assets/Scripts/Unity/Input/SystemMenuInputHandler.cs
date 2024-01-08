using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.Unity.Behaviours;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Input
{
    public class SystemMenuInputHandler : BaseViewInputHandler
    {
        public SystemMenuInputHandler(ViewManager viewManager) : base(viewManager) { }

        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("SystemMenuInputHandler.OnKeyPressed");

            if (processCommonKey(key))
                return;

            var keyboard = Keyboard.current;

            if (key == keyboard.escapeKey)
            {
                //EventManager.UIRequestEvent.Invoke(new ResetViewRequest());
                EventManager.Publish(new UIRequest(UIRequest.Command.ResetView));
            }
        }
    }
}
