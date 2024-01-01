using UnityEngine.InputSystem.Controls;
using Ventura.Util;
using Ventura.Unity.Behaviours;
using UnityEngine.InputSystem;

namespace Ventura.Unity.Input
{
    public class SkillsInputHandler : AbstractViewInputHandler
    {
        public SkillsInputHandler(ViewManager viewManager) : base(viewManager) { }

        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("SkillsInputHandler.OnKeyPressed");

            if (processCommonKey(key))
                return;

            var keyboard = Keyboard.current;

            if (key == keyboard.escapeKey)
            {
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
            }

            //do nothing
        }

    }
}
