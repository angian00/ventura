using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Behaviours;
using Ventura.Unity.Events;

namespace Ventura.Unity.Input
{

    public class InventoryInputHandler : AbstractViewInputHandler
    {
        public InventoryInputHandler(ViewManager viewManager) : base(viewManager) { }

        public override void OnKeyPressed(KeyControl key)
        {
            //DebugUtils.Log("InventoryInputHandler .OnKeyPressed");

            if (processCommonKey(key))
                return;

            var keyboard = Keyboard.current;
            ActionData? newActionData = null;

            if (key == keyboard.escapeKey)
            {
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
            }
            else if (key == keyboard.uKey)
            {
                //FUTURE: use focused object
            }
            else
            {
                //ignore keyPressed
            }

            if (newActionData != null)
                EventManager.ActionRequestEvent.Invoke(newActionData);
        }
    }
}
