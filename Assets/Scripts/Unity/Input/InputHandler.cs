using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.Unity.Behaviours;
using Ventura.Util;

namespace Ventura.Unity.Input
{
    public abstract class ViewInputHandler
    {
        protected ViewManager _viewManager;

        public ViewInputHandler(ViewManager viewManager)
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

            if (key == keyboard.qKey)
            {
                GameManager.Instance.ExitGame();
                //TODO: viewManager.SwitchTo(ViewManager.ViewId.Confirmation);
                processed = true;
            }
            if (key == keyboard.iKey)
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
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
                processed = true;
            }
            else if (key == keyboard.escapeKey)
            {
                //FUTURE: pop-up confirmation to main menu
                _viewManager.SwitchTo(ViewManager.ViewId.Map);
                processed = true;
            }

            return processed;
        }

    }
}
