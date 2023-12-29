using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
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


        private GameObject _playerObj;
        private GameObject _cameraObj;


        private Transform _terrainLayer;
        private Transform _sitesLayer;
        private Transform _fogLayer;

        private GameObject[,] _fogTiles;

        private Orchestrator _orch;


        void Start()
        {
            _terrainLayer = transform.Find("TerrainLayer");
            _sitesLayer = transform.Find("SitesLayer");
            _fogLayer = transform.Find("FogLayer");

            _playerObj = GameObject.Find("Player");
            _cameraObj = GameObject.Find("Map Camera");

            _orch = Orchestrator.GetInstance();
        }

        void Update()
        {
            foreach (var pendingType in _orch.PendingUpdates)
            {
                switch (pendingType)
                {
                    case Orchestrator.PendingType.Terrain:
                        updateTerrain();
                        break;

                    case Orchestrator.PendingType.Player:
                        updatePlayer();
                        break;
                }
            }

            _orch.PendingUpdates.Clear();
        }


        private void updateTerrain()
        {
            Messages.Log("MapManager.updateTerrain()");

            rebuildMap(_orch.CurrMap);

            //update ui location info
            string locationInfoStr = "";
            var mapNames = _orch.World.GetStackMapNames();

            for (int i = mapNames.Count - 1; i >= 0; i--)
            {
                locationInfoStr += mapNames[i];

                if (i > 0)
                    locationInfoStr += " > ";
            }

            //FIXME: choose if this goes to UI Manager or UIManager.UpdateTileInfo goes here
            var textObj = GameObject.Find("Location Info");
            textObj.GetComponent<TextMeshProUGUI>().text = locationInfoStr;
        }


        private void updatePlayer()
        {
            //Messages.Log("MapManager.updatePlayer()");

            var playerX = _orch.Player.x;
            var playerY = _orch.Player.y;

            var targetObjPos = _playerObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            _playerObj.transform.position = targetObjPos;

            targetObjPos = _cameraObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            _cameraObj.transform.position = targetObjPos;

            updateFog(_orch.CurrMap);
        }


        private void rebuildMap(GameMap gameMap)
        {
            Messages.Log("MapManager.rebuildMap()");

            UnityUtils.RemoveAllChildren(_terrainLayer);
            UnityUtils.RemoveAllChildren(_sitesLayer);
            UnityUtils.RemoveAllChildren(_fogLayer);

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
            updateFog(gameMap);


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

        private void updateFog(GameMap gameMap)
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
