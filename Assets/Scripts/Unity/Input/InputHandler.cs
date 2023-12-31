using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.Unity.Behaviours;

namespace Ventura.Unity.Input
{
    public abstract class ViewInputHandler
    {
        protected ViewManager _viewManager;

        protected ViewInputHandler(ViewManager viewManager)
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
            if (key == keyboard.escapeKey)
            {
                //FUTURE: system command menu
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
                processed = true;
            }
            else if (key == keyboard.nKey)
            {
                //SystemManager.Instance.ExecuteCommand(SystemManager.Command.New);
                _viewManager.ShowPopup("New Game", SystemManager.Command.New);
                processed = true;
            }
            else if (key == keyboard.qKey)
            {
                //SystemManager.Instance.ExecuteCommand(SystemManager.Command.Exit);
                _viewManager.ShowPopup("Exit Game", SystemManager.Command.Exit);
                processed = true;
            }
            
            //------------------- view toggle commands -------------------
            else if (key == keyboard.iKey)
            {
                _viewManager.Toggle(ViewManager.ViewId.Inventory);
                processed = true;
            }
            else if (key == keyboard.sKey)
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
