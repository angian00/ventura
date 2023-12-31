using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get => _instance; }


        private Orchestrator _orch;

        void Awake()
        {
            _instance = this;

            _orch = Orchestrator.Instance;
            _orch.NewGame();
        }

        void Start()
        {
            StatusLineManager.Instance.Display("Welcome, adventurer!");
        }


        void Update()
        {
            //NB: It is important that GameManager is set to higher priority than default in Unity Editor --> Project Settings --> Script Execution Order.
            //    This guarantees the following order or execution:
            //    (1) GameManager.Update
            //          Orchestrator.ProcessTurn    --> write PendingUpdates
            //    (2) <other MonoBehaviors>.Update  --> read PendingUpdates
            //    (3) GameManager.LateUpdate        --> clear PendingUpdates

            _orch.ProcessTurn();
        }

        void LateUpdate()
        {
            PendingUpdates.Instance.Clear();
        }


        public void ExitGame()
        {
            DebugUtils.Log("Exiting Game");
            //diffrent calls needed if application is run in Unity editor or as a standalone application
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

