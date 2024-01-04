using System.Collections.ObjectModel;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Unity.Events;
using Ventura.Unity.Graphics;
using Ventura.Util;


namespace Ventura.Unity.Behaviours
{

    public class MainViewBehaviour : MonoBehaviour
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


        private void OnEnable()
        {
            EventManager.LocationChangeEvent.AddListener(onLocationChange);
            EventManager.MapUpdateEvent.AddListener(onMapDataUpdate);
            EventManager.ActorUpdateEvent.AddListener(onPlayerDataUpdate);
        }

        private void OnDisable()
        {
            EventManager.LocationChangeEvent.RemoveListener(onLocationChange);
            EventManager.MapUpdateEvent.RemoveListener(onMapDataUpdate);
            EventManager.ActorUpdateEvent.RemoveListener(onPlayerDataUpdate);
        }


        private void onLocationChange(GameMap mapData, ReadOnlyCollection<string> mapStackNames)
        {
            updateTiles(mapData);
        }

        private void onMapDataUpdate(GameMap mapData)
        {
            updateFog(mapData);
        }

        private void onPlayerDataUpdate(Actor a)
        {
            if (!(a is Player))
                return;

            updatePlayer((Player)a);
        }

        // ------ mouse input handlers -------------------------
        public void OnTileClick(Vector2Int tilePos)
        {
            //FUTURE: OnTileClick
        }
        
        public void OnTileMouseEnter(Vector2Int tilePos)
        {
            sendTileInfo(tilePos);
        }

        public void OnTileMouseExit(Vector2Int tilePos)
        {
            sendTileInfo(null);
        }

        private void sendTileInfo(Vector2Int? tilePos)
        {
            EventManager.UIRequestEvent.Invoke(new MapTilePointerRequest(tilePos));
        }


    // -----------------------------------------------------


    private void updatePlayer(Player playerData)
        {
            //GameDebugging.Log("MainViewBehaviour.updatePlayer()");

            var playerX = playerData.x;
            var playerY = playerData.y;

            var targetObjPos = playerObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            playerObj.transform.position = targetObjPos;

            targetObjPos = cameraObj.transform.position;
            targetObjPos.x = playerX;
            targetObjPos.y = playerY;
            cameraObj.transform.position = targetObjPos;

            //updateFog(gameState.CurrMap); //FIXME
        }


        private void updateTiles(GameMap gameMap)
        {
            //DebugUtils.Log("MainViewBehaviour.updateTiles()");

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
                    newMapTile.GetComponent<MapTileBehaviour>().mapManager = this;
                    newMapTile.GetComponent<MapTileBehaviour>().MapPos = new Vector2Int(x, y);
                    newMapTile.GetComponent<SpriteRenderer>().color = GraphicsConfig.TerrainColors[terrainType];
                    newMapTile.transform.SetParent(terrainLayer);

                    var newFogTile = Instantiate(fogTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newFogTile.GetComponent<SpriteRenderer>().color = fogColor;
                    newFogTile.transform.SetParent(fogLayer);

                    _fogTiles[x, y] = newFogTile;
                }
            }

            buildSites(gameMap);

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


        private void updateFog(GameMap gameMap)
        {
            //GameDebugging.Log("MainViewBehaviour.UpdateFog()");

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