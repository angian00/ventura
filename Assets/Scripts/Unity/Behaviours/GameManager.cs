using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Unity.Behaviours
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get => _instance; }


        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            SystemManager.Instance.ExecuteCommand(SystemManager.Command.New);
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

            Orchestrator.Instance.ProcessTurn();
        }

        void LateUpdate()
        {
            Orchestrator.Instance.PendingUpdates.Clear();
        }
    }
}
