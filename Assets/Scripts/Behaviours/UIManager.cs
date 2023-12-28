
using UnityEngine;


namespace Ventura.Behaviours
{

    public class UIManager : MonoBehaviour
    {
        private GameObject tileInfo;

        void Start()
        {
            tileInfo = GameObject.Find("Tile Info");
        }

        public void UpdateTileInfo(string text)
        {
            tileInfo.GetComponent<TMPro.TextMeshProUGUI>().text = text;
        }
    }
}
