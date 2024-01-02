
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

        void Awake()
        {
            _instance = this;
        }

        public void UpdateTileInfo(GameMap gameMap, Vector2Int? pos)
        {
            //DebugUtils.Log($"UIManager.UpdateTileInfo()");

            if (pos == null || !gameMap.IsInBounds(((Vector2Int)pos).x, ((Vector2Int)pos).y))
            {
                tileInfo1.text = "";
                tileInfo2.text = "";
                return;
            }

            tileInfo1.text = getTileInfo(gameMap, (Vector2Int)pos);
            tileInfo2.text = getEntityInfo(gameMap, (Vector2Int)pos);
        }


        private string getTileInfo(GameMap gameMap, Vector2Int pos)
        {
            var res = $"x: {pos.x}, y: {pos.y}";

            if (gameMap.Explored[pos.x, pos.y])
                res += $" - {gameMap.Terrain[pos.x, pos.y].Label}";

            return res;
        }


        private string getEntityInfo(GameMap gameMap, Vector2Int pos)
        {
            if (!gameMap.Visible[pos.x, pos.y])
                return "";

            var a = gameMap.GetAnyEntityAt<Actor>(pos);
            if (a != null)
                return a.Name;

            var s = gameMap.GetAnyEntityAt<Site>(pos.x, pos.y);
            if (s != null)
                return s.Name;

            return "";
        }
    }
}
