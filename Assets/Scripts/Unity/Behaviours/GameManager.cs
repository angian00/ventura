using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Unity.Behaviours
{
    public class GameManager : MonoBehaviour
    {
        void Update()
        {
            Orchestrator.Instance.ProcessTurn(); //TODO: move somewhere
        }
    }
}
