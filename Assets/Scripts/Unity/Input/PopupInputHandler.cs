using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Ventura.Unity.Behaviours;

namespace Ventura.Unity.Input
{
    public class PopupInputHandler : AbstractViewInputHandler
    {
        private PopupManager _popupManager;

        public PopupInputHandler(ViewManager viewManager, PopupManager popupManager) : base(viewManager)
        {
            this._popupManager = popupManager;
        }


        public override void OnKeyPressed(KeyControl key)
        {
            //DebugUtils.Log("MapInputHandler.OnKeyPressed");

            var keyboard = Keyboard.current;

            if (key == keyboard.escapeKey)
            {
                _viewManager.HidePopup();
            }
            else if (key == keyboard.yKey)
            {
                _popupManager.OnYes();
            }
            else if (key == keyboard.nKey)
            {
                _popupManager.OnNo();
            }
        }
    }
}
