using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Ventura.Unity.Input
{
    public abstract class BaseViewInputHandler
    {
        //protected ViewManager _viewManager;

        //protected BaseViewInputHandler(ViewManager viewManager)
        //{
        //    this._viewManager = viewManager;
        //}

        public virtual void OnKeyPressed(KeyControl key)
        {
            //if (processCommonKey(key))
            //    return;
        }


        ///**
        // * Returns true if key is intercepted
        // */
        protected bool processCommonKey(KeyControl key)
        {
            var keyboard = Keyboard.current;
            var processed = false;

            //    //------------------- system commands -------------------
            //    //if (key == keyboard.nKey)
            //    //{
            //    //    _viewManager.showPopup("New Game", SystemRequest.Command.New);
            //    //    processed = true;
            //    //}
            //    //else if (key == keyboard.qKey)
            //    //{
            //    //    _viewManager.showPopup("Exit Game", SystemRequest.Command.Exit);
            //    //    processed = true;
            //    //}
            //    //else if (key == keyboard.sKey)
            //    //{
            //    //    _viewManager.showPopup("Save Game", SystemRequest.Command.Save);
            //    //    processed = true;
            //    //}
            //    //else if (key == keyboard.lKey)
            //    //{
            //    //    _viewManager.showPopup("Load Game", SystemRequest.Command.Load);
            //    //    processed = true;
            //    //}

            //    //------------------- view showUIView commands -------------------
            //    //else if (key == keyboard.iKey)
            //    if (key == keyboard.iKey)
            //    {
            //        EventManager.Publish(new ShowUIViewRequest(ViewManager.ViewId.Inventory));
            //        processed = true;
            //    }
            //    else if (key == keyboard.kKey)
            //    {
            //        EventManager.Publish(new ShowUIViewRequest(ViewManager.ViewId.Character));
            //        processed = true;
            //    }
            //    else if (key == keyboard.mKey)
            //    {
            //        //NB: map view doesn't have a showUIView behaviour,
            //        //    it is the default view
            //        //_viewManager.switchTo(ViewManager.ViewId.Map);
            //        processed = true;
            //    }


            return processed;
        }

    }
}
