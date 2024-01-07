using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ventura.GameLogic;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class SystemMenuBehaviour : MonoBehaviour
    {
        public enum SystemMenuCommand
        {
            New,
            Exit,
            Load,
            Save,
            Resume,
            Settings,
        }


        public GameObject systemMenuButtonTemplate;
        public Transform buttonContainer;

        public List<string> buttonNames;
        public List<SystemMenuCommand> buttonCommands;

        [Tooltip("Subset of buttonNames matching buttons that must be enabled only if the game is already active")]
        public List<string> activeOnlyNames;

        private Dictionary<string, GameObject> _buttonObjs = new();
        private bool _isGameScene = false;


        void Awake()
        {
            checkConfig();

            _buttonObjs.Clear();
            for (int i = 0; i < buttonNames.Count; i++)
            {
                //var newButton = Instantiate(systemMenuButtonTemplate);
                var newButton = Instantiate(systemMenuButtonTemplate, new Vector3(0, 0), Quaternion.identity);
                //var newButtonScript = newButton.GetComponent<SystemMenuButtonBehaviour>();
                //newButtonScript.buttonLabel.text = buttonNames[i];
                getButtonLabel(newButton).text = buttonNames[i];
                var command = buttonCommands[i];
                newButton.GetComponent<Button>().onClick.AddListener(() => { processSystemMenuCommand(command); });
                newButton.transform.SetParent(buttonContainer, false);

                _buttonObjs.Add(buttonNames[i], newButton);
            }

            _isGameScene = (SceneManager.GetActiveScene().name == UnityUtils.GAME_SCENE_NAME);
            toggleActiveButtons(_isGameScene);
        }


        private void checkConfig()
        {
            if (buttonNames.Count != buttonCommands.Count)
                throw new GameException("buttonNames and buttonCommands configuration must be consistent");

            foreach (var buttonLabel in activeOnlyNames)
            {
                if (!buttonNames.Contains(buttonLabel))
                    throw new GameException($"Button label [{buttonLabel}] found in activeOnlyNames not present in buttonNames");
            }
        }


        private void toggleActiveButtons(bool isGameActive)
        {
            foreach (var buttonText in _buttonObjs.Keys)
            {
                var buttonObj = _buttonObjs[buttonText];

                var isButtonEnabled = true;
                if ((!isGameActive) && activeOnlyNames.Contains(getButtonLabel(buttonObj).text))
                    isButtonEnabled = false;

                buttonObj.GetComponent<Button>().interactable = isButtonEnabled;
            }
        }

        private TextMeshProUGUI getButtonLabel(GameObject button)
        {
            return button.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        }

        private void processSystemMenuCommand(SystemMenuCommand command)
        {
            switch (command)
            {
                case SystemMenuCommand.New:
                    if (_isGameScene)
                        EventManager.Publish(new AskYesNoRequest("New Game", SystemRequest.Command.New));
                    else
                        EventManager.Publish(new SystemRequest(SystemRequest.Command.New));
                    break;

                case SystemMenuCommand.Exit:
                    if (_isGameScene)
                        EventManager.Publish(new AskYesNoRequest("Exit", SystemRequest.Command.Exit));
                    else
                        EventManager.Publish(new SystemRequest(SystemRequest.Command.Exit));
                    break;

                case SystemMenuCommand.Load:
                    if (_isGameScene)
                        EventManager.Publish(new AskYesNoRequest("Load Game", SystemRequest.Command.Load));
                    else
                        EventManager.Publish(new SystemRequest(SystemRequest.Command.Load));
                    break;

                case SystemMenuCommand.Save:
                    EventManager.Publish(new SystemRequest(SystemRequest.Command.Save));
                    //EventManager.UIRequestEvent.Invoke(new ResetViewRequest());
                    EventManager.Publish(new UIRequest(UIRequest.Command.ResetView));
                    break;

                case SystemMenuCommand.Resume:
                    //EventManager.UIRequestEvent.Invoke(new ResetViewRequest());
                    EventManager.Publish(new UIRequest(UIRequest.Command.ResetView));
                    break;

                case SystemMenuCommand.Settings:
                    //TODO: show settings
                    break;

                default:
                    //no more cases
                    break;
            }
        }

    }
}
