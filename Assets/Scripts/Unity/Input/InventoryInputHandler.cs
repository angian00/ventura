using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Behaviours;

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
            var orch = Orchestrator.Instance;
            GameAction? newAction = null;

            if (key == keyboard.uKey)
            {
                //newAction = new UseAction(orch, orch.MapPlayerPos, orch.MapPlayerPos.Inventory.Items[0]); //DEBUG
            }
            else
            {
                //ignore keyPressed
            }


            if (newAction != null)
                orch.EnqueuePlayerAction(newAction);
        }
    }
}
