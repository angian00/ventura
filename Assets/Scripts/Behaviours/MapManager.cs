using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;


namespace Ventura.Behaviours
{

    public class MapManager : MonoBehaviour
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


        public void ClearMap()
        {
            UnityUtils.RemoveAllChildren(_terrainLayer);
            UnityUtils.RemoveAllChildren(_sitesLayer);
            UnityUtils.RemoveAllChildren(_fogLayer);

            _fogTiles = new GameObject[,] { { } };
        }


        public void InitMap(GameMap gameMap)
        {
            Messages.Log("MapManager.InitMap()");

            _fogTiles = new GameObject[gameMap.Width, gameMap.Height];

            for (int x = 0; x < gameMap.Width; x++)
            {
                for (int y = 0; y < gameMap.Height; y++)
                {
                    TerrainType terrainType = gameMap.Terrain[x, y];

                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newMapTile.GetComponent<SpriteRenderer>().color = GraphicsConfig.TerrainColors[terrainType];
                    newMapTile.transform.SetParent(_terrainLayer);


                    var newFogTile = Instantiate(fogTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newFogTile.GetComponent<SpriteRenderer>().color = fogColor;
                    newFogTile.transform.SetParent(_fogLayer);

                    _fogTiles[x, y] = newFogTile;
                }
            }
            UpdateFog(gameMap);


            foreach (var e in gameMap.Entities)
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

                        var newEntity = Instantiate(entityTemplate, new Vector3(e.x, e.y), Quaternion.identity);
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

        public void UpdateFog(GameMap gameMap)
        {
            //Messages.Log("MapManager.UpdateFog()");

            for (int x = 0; x < gameMap.Width; x++)
            {
                for (int y = 0; y < gameMap.Height; y++)
                {
                    float alpha;

                    if (gameMap.Visible[x, y])
                        alpha = 0.0f;
                    else if (gameMap.Explored[x, y])
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
