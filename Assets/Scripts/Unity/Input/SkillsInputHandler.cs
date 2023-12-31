using UnityEngine.InputSystem.Controls;
using Ventura.Util;
using Ventura.Unity.Behaviours;
using UnityEngine;

namespace Ventura.Unity.Input
{
    public class SkillsInputHandler : ViewInputHandler
    {
        public SkillsInputHandler(ViewManager viewManager) : base(viewManager) { }


        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("SkillsInputHandler.OnKeyPressed");

            if (processCommonKey(key))
                return;

            //do nothing
        }

    }
}
