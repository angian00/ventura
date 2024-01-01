using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Unity.Graphics;
using Ventura.Util;
using static UnityEditor.PlayerSettings;


namespace Ventura.Unity.Behaviours
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

        public GameObject playerObj;
        public GameObject cameraObj;
        public Transform terrainLayer;
        public Transform sitesLayer;
        public Transform fogLayer;
        public BoxCollider2D mapCollider;


        private GameObject[,] _fogTiles;
        private Orchestrator _orch;


        void Start()
        {
            _orch = Orchestrator.Instance;
        }

        void Update()
        {
            foreach (var pendingType in PendingUpdates.Instance.GetAll())
            {
                switch (pendingType)
                {
                    case PendingUpdateId.MapTerrain:
                        updateTerrain();
                        break;

                    case PendingUpdateId.MapPlayerPos:
                        updatePlayer();
                        break;
                }
            }
        }

        public void OnTileClick(Vector2Int tilePos)
        {
            //FUTURE: OnTileClick
        }
        
        public void OnTileMouseEnter(Vector2Int tilePos)
        {
            var newAction = new LookAction(_orch, _orch.Player, tilePos);
            _orch.EnqueuePlayerAction(newAction);
        }

        public void OnTileMouseExit(Vector2Int tilePos)
        {
            var newAction = new LookAction(_orch, _orch.Player, null);
            _orch.EnqueuePlayerAction(newAction);
        }


        private void updateTerrain()
        {
            DebugUtils.Log("MapManager.updateTerrain()");

            buildTiles(_orch.CurrMap);
            updateLocationInfo();
        }

        private void updatePlayer()
        {
            //GameDebugging.Log("MapManager.updatePlayer()");

            var playerX = _orch.Player.x;
            var playerY = _orch.Player.y;

            var targetObjPos = playerObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            playerObj.transform.position = targetObjPos;

            targetObjPos = cameraObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            cameraObj.transform.position = targetObjPos;

            updateFog(_orch.CurrMap);
        }


        private void buildTiles(GameMap gameMap)
        {
            //DebugUtils.Log("MapManager.buildTiles()");

            UnityUtils.RemoveAllChildren(terrainLayer);
            UnityUtils.RemoveAllChildren(sitesLayer);
            UnityUtils.RemoveAllChildren(fogLayer);

            _fogTiles = new GameObject[gameMap.Width, gameMap.Height];

            for (int x = 0; x < gameMap.Width; x++)
            {
                for (int y = 0; y < gameMap.Height; y++)
                {
                    TerrainType terrainType = gameMap.Terrain[x, y];

                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newMapTile.GetComponent<MapTileManager>().mapManager = this;
                    newMapTile.GetComponent<MapTileManager>().mapPos = new Vector2Int(x, y);
                    newMapTile.GetComponent<SpriteRenderer>().color = GraphicsConfig.TerrainColors[terrainType];
                    newMapTile.transform.SetParent(terrainLayer);

                    var newFogTile = Instantiate(fogTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newFogTile.GetComponent<SpriteRenderer>().color = fogColor;
                    newFogTile.transform.SetParent(fogLayer);

                    _fogTiles[x, y] = newFogTile;
                }
            }

            buildSites(gameMap);

            //updateCollider(gameMap);
            updateFog(gameMap);
        }


        private void buildSites(GameMap gameMap)
        {
            foreach (var e in gameMap.Entities)
            {
                if (!(e is Site))
                    continue;

                var spriteName = GraphicsConfig.EntityIcons["site"];
                var sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
                if (sprite == null)
                {
                    DebugUtils.Log($"!! Sprite not found: {spriteName}");
                    continue;
                }

                var newEntity = Instantiate(entityTemplate, new Vector3(e.x, e.y), Quaternion.identity);
                newEntity.name = e.Name; //FIXME: there is no guarantee that entity name is unique
                newEntity.GetComponent<SpriteRenderer>().sprite = sprite;
                newEntity.transform.SetParent(sitesLayer);
            }
        }

        private void updateCollider(GameMap gameMap)
        {
            mapCollider.size = new Vector2Int(gameMap.Width, gameMap.Height);
            mapCollider.offset = new Vector2Int(gameMap.Width / 2, gameMap.Height / 2); //needed for some reason
        }


        private void updateFog(GameMap gameMap)
        {
            //GameDebugging.Log("MapManager.UpdateFog()");

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

        private void updateLocationInfo()
        {
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

    }
}
