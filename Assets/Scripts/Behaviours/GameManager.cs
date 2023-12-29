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
            StatusLineManager.DisplayStatus("Welcome, adventurer!");
        }


        void Update()
        {
            _orch.ProcessTurn();
        }
    }
}

