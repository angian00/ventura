using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.Unity.Behaviours;
using Ventura.Unity.Events;

namespace Ventura.Unity.Input
{
    public abstract class BaseViewInputHandler
    {
        protected ViewManager _viewManager;

        protected BaseViewInputHandler(ViewManager viewManager)
        {
            this._viewManager = viewManager;
        }

        public virtual void OnMouseMove(Vector2 mousePos, Camera camera)
        {
            //do nothing
        }

        public virtual void OnKeyPressed(KeyControl key)
        {
            if (processCommonKey(key))
                return;
        }


        /**
         * Returns true if key is intercepted
         */
        protected bool processCommonKey(KeyControl key)
        {
            var keyboard = Keyboard.current;
            var processed = false;

            //------------------- system commands -------------------
            if (key == keyboard.nKey)
            {
                _viewManager.ShowPopup("New Game", SystemRequest.Command.New);
                processed = true;
            }
            else if (key == keyboard.qKey)
            {
                _viewManager.ShowPopup("Exit Game", SystemRequest.Command.Exit);
                processed = true;
            }
            else if (key == keyboard.sKey)
            {
                _viewManager.ShowPopup("Save Game", SystemRequest.Command.Save);
                processed = true;
            }
            else if (key == keyboard.lKey)
            {
                _viewManager.ShowPopup("Load Game", SystemRequest.Command.Load);
                processed = true;
            }

            //------------------- view toggle commands -------------------
            else if (key == keyboard.iKey)
            {
                _viewManager.Toggle(ViewManager.ViewId.Inventory);
                processed = true;
            }
            else if (key == keyboard.kKey) //FUTURE: choose sensible key mapping
            {
                _viewManager.Toggle(ViewManager.ViewId.Skills);
                processed = true;
            }
            else if (key == keyboard.mKey)
            {
                //NB: map view doesn't have a toggle behaviour,
                //    it is the default view
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
                processed = true;
            }


            return processed;
        }

    }
}
