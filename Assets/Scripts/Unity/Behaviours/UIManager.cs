
using TMPro;
using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Unity.Behaviours
{

    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance { get => _instance; }

        public TextMeshProUGUI tileInfo1;
        public TextMeshProUGUI tileInfo2;

        private Orchestrator _orch;

        void Awake()
        {
            _instance = this;
            _orch = Orchestrator.Instance;
        }

        public void UpdateTileInfo(Vector2Int? pos)
        {
            //DebugUtils.Log($"UIManager.UpdateTileInfo()");

            if (pos == null || !_orch.CurrMap.IsInBounds(((Vector2Int)pos).x, ((Vector2Int)pos).y))
            {
                tileInfo1.text = "";
                tileInfo2.text = "";
                return;
            }

            tileInfo1.text = getTileInfo((Vector2Int)pos);
            tileInfo2.text = getEntityInfo((Vector2Int)pos);
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

            var a = _orch.CurrMap.GetAnyEntityAt<Actor>(pos);
            if (a != null)
                return a.Name;

            var s = _orch.CurrMap.GetAnyEntityAt<Site>(pos.x, pos.y);
            if (s != null)
                return s.Name;

            return "";
        }
    }
}
