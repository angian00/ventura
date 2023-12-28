using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;


namespace Ventura.Behaviours
{

    public class BoardManager : MonoBehaviour
    {
        public GameObject terrainTileTemplate;
        public GameObject fogTileTemplate;
        public GameObject entityTemplate;

        public Color fogColor = Color.grey;

        [Range(0.0f, 1.0f)]
        public float fogExploredAlpha = 0.2f;

        [Range(0.0f, 1.0f)]
        public float fogUnexploredAlpha = 0.9f;


        private Transform _terrainLayer;
        private Transform _sitesLayer;
        private Transform _fogLayer;

        private GameObject[,] _fogTiles;


        private void Start()
        {
            _terrainLayer = transform.Find("TerrainLayer");
            _sitesLayer = transform.Find("SitesLayer");
            _fogLayer = transform.Find("FogLayer");

            _fogTiles = new GameObject[,] { { } };
        }


        public void ClearBoard()
        {
            UnityUtils.RemoveAllChildren(_terrainLayer);
            UnityUtils.RemoveAllChildren(_sitesLayer);
            UnityUtils.RemoveAllChildren(_fogLayer);

            _fogTiles = new GameObject[,] { { } };
        }


        public void InitBoard(GameMap map)
        {
            Messages.Log("BoardManager.InitBoard()");

            _fogTiles = new GameObject[map.Width, map.Height];

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    TerrainType terrainType = map.Terrain[x, y];

                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newMapTile.GetComponent<SpriteRenderer>().color = GraphicsConfig.TerrainColors[terrainType];
                    newMapTile.transform.SetParent(_terrainLayer);


                    var newFogTile = Instantiate(fogTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newFogTile.GetComponent<SpriteRenderer>().color = fogColor;
                    newFogTile.transform.SetParent(_fogLayer);

                    _fogTiles[x, y] = newFogTile;
                }
            }
            UpdateFog(map);


            foreach (var e in map.Entities)
            {
                if (e.Name == "player")
                    continue;

                switch (e.GetType().Name)
                {
                    case "Site":
                        var spriteName = GraphicsConfig.EntityIcons["site"];
                        var sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
                        if (sprite == null)
                        {
                            Messages.Log($"!! Sprite not found: {spriteName}");
                            continue;
                        }

                        var newEntity = Instantiate(entityTemplate, new Vector3(e.X, e.Y), Quaternion.identity);
                        newEntity.name = e.Name; //FIXME: there is no guarantee that entity name is unique
                        newEntity.GetComponent<SpriteRenderer>().sprite = sprite;
                        newEntity.transform.SetParent(_sitesLayer);
                        break;

                    default:
                        Messages.Log($"No icon for entity type {e.GetType().Name}");
                        break;
                }
            }
        }

        public void UpdateFog(GameMap map)
        {
            Messages.Log("BoardManager.UpdateFog()");

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    float alpha;

                    if (map.Visible[x, y])
                        alpha = 0.0f;
                    else if (map.Explored[x, y])
                        alpha = fogExploredAlpha;
                    else
                        alpha = fogUnexploredAlpha;

                    var tileColor = fogColor;
                    tileColor.a = alpha;

                    _fogTiles[x, y].GetComponent<SpriteRenderer>().color = tileColor;
                }
            }
        }
    }
}
