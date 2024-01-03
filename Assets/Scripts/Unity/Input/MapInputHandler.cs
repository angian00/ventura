using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Behaviours;
using UnityEngine;
using Ventura.Util;
using Ventura.Unity.Events;

namespace Ventura.Unity.Input
{
    public class MapInputHandler : AbstractViewInputHandler
    {
        //private Vector2Int? _lastPos = null;

        public MapInputHandler(ViewManager viewManager) : base(viewManager) { }


        public override void OnKeyPressed(KeyControl key)
        {
            DebugUtils.Log("MapInputHandler.OnKeyPressed");

            if (processCommonKey(key))
                return;

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
            else if (key == keyboard.gKey)
            {
                newActionData = new ActionData(GameActionType.PickupItemAction);
            }
            //else if (key == keyboard.escapeKey)
            //{
            //    //TODO: open system _command menu
            //}

            else
            {
                //ignore keyPressed
            }

            if (newActionData != null)
                EventManager.ActionRequestEvent.Invoke(newActionData);
        }
    }
}
