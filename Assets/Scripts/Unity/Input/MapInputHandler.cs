using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Behaviours;
using UnityEngine;
using Ventura.Util;

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
            Actor player = Orchestrator.Instance.GameState.Player;
            GameAction? newAction = null;

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
                newAction = new BumpAction(player, deltaX, deltaY);

            }
            else if (key == keyboard.numpad5Key)
            {
                newAction = new WaitAction(player);
            }
            else if (key == keyboard.gKey)
            {
                newAction = new PickupAction(player);
            }
            else if (key == keyboard.escapeKey)
            {
                //TODO: open system _command menu
            }

            else
            {
                //ignore keyPressed
            }

            if (newAction != null)
                Orchestrator.Instance.EnqueuePlayerAction(newAction);
        }


        //public override void OnMouseMove(Vector2 mousePos, Camera camera)
        //{
        //    var currPos = getTilePos(mousePos, camera);
        //    if (currPos != _lastPos)
        //    {
        //        _lastPos = currPos;
        //        var orch = Orchestrator.Instance;
        //        var newAction = new LookAction(orch, orch.Player, currPos);
        //        orch.EnqueuePlayerAction(newAction);
        //    }
        //}

        //private Vector2Int? getTilePos(Vector2 mousePos, Camera mapCamera)
        //{
        //    var worldMousePos = mapCamera.ScreenToWorldPoint(mousePos);

        //    return new Vector2Int((int)Math.Round(worldMousePos.x), (int)Math.Round(worldMousePos.y));
        //}
    }
}
