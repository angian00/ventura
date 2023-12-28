
using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Behaviours
{

    public class UIManager : MonoBehaviour
    {
        private GameObject _tileInfo1;
        private GameObject _tileInfo2;

        private Orchestrator _orch;


        void Start()
        {
            _tileInfo1 = GameObject.Find("Tile Info 1");
            _tileInfo2 = GameObject.Find("Tile Info 2");

            _orch = Orchestrator.GetInstance();
        }

        public void UpdateTileInfo(Vector2Int? pos)
        {
            if (pos == null)
            {
                _tileInfo1.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                _tileInfo2.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                return;
            }

            _tileInfo1.GetComponent<TMPro.TextMeshProUGUI>().text = getTileInfo((Vector2Int)pos);
            _tileInfo2.GetComponent<TMPro.TextMeshProUGUI>().text = getEntityInfo((Vector2Int)pos);
        }


        private string getTileInfo(Vector2Int pos)
        {
            var res = $"x: {pos.x}, y: {pos.y}";

            if (_orch.CurrMap.Explored[pos.x, pos.y])
                res += $" - {_orch.CurrMap.Terrain[pos.x, pos.y].Label}";

            return res;
        }


        private string getEntityInfo(Vector2Int pos)
        {
            if (!_orch.CurrMap.Visible[pos.x, pos.y])
                return "";

            var a = _orch.CurrMap.GetActorAt(pos.x, pos.y);
            if (a != null)
                return a.Name;

            var s = _orch.CurrMap.GetSiteAt(pos.x, pos.y);
            if (s != null)
                return s.Name;

            return "";
        }
    }
}
