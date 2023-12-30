using UnityEngine;
using Ventura.GameLogic;


namespace Ventura.Behaviours
{
    public class GameManager : MonoBehaviour
    {
        private Orchestrator _orch;

        void Awake()
        {
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
    }
}

