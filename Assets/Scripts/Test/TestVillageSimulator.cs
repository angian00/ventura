using UnityEngine;


namespace Ventura.Test
{


    public class TestVillageSimulator : MonoBehaviour
    {


        private void Awake()
        {
        }

        public void OnButtonClicked()
        {
            runVillageSimulation();
        }

        private void runVillageSimulation()
        {
            var simulator = new VillageSimulator.VillageSimulator();
            simulator.PrintState();

        }
    }
}
