using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Events;

namespace Ventura.Unity.Input
{
    public class MapInputHandler : BaseViewInputHandler
    {
        //private Vector2Int? _lastPos = null;

        //public MapInputHandler(ViewManager viewManager) : base(viewManager) { }


        public override void OnKeyPressed(KeyControl key)
        {
            //DebugUtils.Log("MapInputHandler.OnKeyPressed");

            var keyboard = Keyboard.current;
            ActionData? newActionData = null;

            int deltaX = 0;
            int deltaY = 0;
            if (key == keyboard.rightArrowKey || key == keyboard.numpad6Key)
                deltaX = 1;
            else if (key == keyboard.leftArrowKey || key == keyboard.numpad4Key)
                deltaX = -1;
            else if (key == keyboard.upArrowKey || key == keyboard.numpad8Key)
                deltaY = 1;
            else if (key == keyboard.downArrowKey || key == keyboard.numpad2Key)
                deltaY = -1;

            else if (key == keyboard.numpad9Key)
            {
                deltaX = 1;
                deltaY = 1;
            }
            else if (key == keyboard.numpad3Key)
            {
                deltaX = 1;
                deltaY = -1;
            }
            else if (key == keyboard.numpad7Key)
            {
                deltaX = -1;
                deltaY = 1;
            }
            else if (key == keyboard.numpad1Key)
            {
                deltaX = -1;
                deltaY = -1;
            }
            if (deltaX != 0 || deltaY != 0)
            {
                newActionData = new ActionData(GameActionType.BumpAction);
                newActionData.DeltaPos = new Vector2Int(deltaX, deltaY);
            }

            else if (key == keyboard.numpad5Key)
            {
                newActionData = new ActionData(GameActionType.WaitAction);
            }
            else if (key == keyboard.commaKey)
            {
                newActionData = new ActionData(GameActionType.ExitMapAction);
            }
            else if (key == keyboard.periodKey)
            {
                newActionData = new ActionData(GameActionType.EnterMapAction);
            }

            else if (key == keyboard.gKey)
            {
                newActionData = new ActionData(GameActionType.PickupItemAction);
            }
            else if (key == keyboard.escapeKey)
            {
                EventManager.Publish(new ShowUIViewRequest(UIRequest.ViewId.System));
            }
            else if (key == keyboard.equalsKey)
            {
                EventManager.Publish(new UIRequest(UIRequest.Command.ZoomIn));
            }
            else if (key == keyboard.minusKey)
            {
                EventManager.Publish(new UIRequest(UIRequest.Command.ZoomOut));
            }

            else
            {
                //ignore keyPressed
            }

            if (newActionData != null)
                EventManager.Publish(new ActionRequest(newActionData));
        }
    }
}
