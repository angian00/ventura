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



        private void Start()
        {
            _terrainLayer = transform.Find("TerrainLayer");
            _sitesLayer = transform.Find("SitesLayer");
            _fogLayer = transform.Find("FogLayer");
        }


        public void ClearBoard()
        {
            UnityUtils.RemoveAllChildren(_terrainLayer);
            UnityUtils.RemoveAllChildren(_sitesLayer);
            UnityUtils.RemoveAllChildren(_fogLayer);
        }


        public void InitBoard(GameMap map)
        {
            Messages.Log("BoardManager.InitBoard");

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    TerrainType terrainType = map.Terrain[x, y];

                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity) as GameObject;
                    newMapTile.GetComponent<SpriteRenderer>().color = GraphicsConfig.TerrainColors[terrainType];
                    newMapTile.transform.SetParent(_terrainLayer);


                    if (map.Visible[x, y])
                        continue;

                    float alpha;
                    if (map.Explored[x, y])
                        alpha = fogExploredAlpha;
                    else
                        alpha = fogUnexploredAlpha;

                    var newFogTile = Instantiate(fogTileTemplate, new Vector3(x, y), Quaternion.identity) as GameObject;
                    var tileColor = fogColor;
                    tileColor.a = alpha;
                    newFogTile.GetComponent<SpriteRenderer>().color = tileColor;
                    newFogTile.transform.SetParent(_fogLayer);
                }
            }

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

                        var newEntity = Instantiate(entityTemplate, new Vector3(e.X, e.Y), Quaternion.identity) as GameObject;
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
    }
}
