using UnityEngine;

namespace Ventura.Behaviours
{

    public class StartMenuButton : MonoBehaviour
    {
        public enum StartMenuCommand
        {
            New,
            Exit,
            Load,
            Save,
        }


        public StartScreenManager startScreenManager;
        public StartMenuCommand command;


        public void OnButtonClick()
        {
            Debug.Log($"StartMenuButton.OnButtonClick; gameObject: {gameObject}");
            switch (command)
            {
                case StartMenuCommand.New:
                    startScreenManager.NewGame();
                    break;
                case StartMenuCommand.Exit:
                    startScreenManager.ExitGame();
                    break;
                case StartMenuCommand.Load:
                    startScreenManager.LoadGame();
                    break;
                case StartMenuCommand.Save:
                    startScreenManager.SaveGame();
                    break;
            }

        }
    }
}
