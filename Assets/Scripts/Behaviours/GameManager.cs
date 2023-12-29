using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Ventura.GameLogic;


namespace Ventura.Behaviours
{
    public class GameManager : MonoBehaviour
    {
        private Orchestrator _orch;

        void Awake()
        {
            _orch = Orchestrator.GetInstance();
            _orch.NewGame();
        }

        void Start()
        {
            StatusLineManager.Instance.Display("Welcome, adventurer!");
        }


        void Update()
        {
            _orch.ProcessTurn();
        }
    }
}

