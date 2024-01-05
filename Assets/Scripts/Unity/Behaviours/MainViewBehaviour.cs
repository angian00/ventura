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
        public Transform itemsLayer;
        public Transform fogLayer;
        public BoxCollider2D mapCollider;


        private GameObject[,] _fogTiles;


        private void OnEnable()
        {
            EventManager.GameStateUpdateEvent.AddListener(onGameStateUpdated);
        }

        private void OnDisable()
        {
            EventManager.GameStateUpdateEvent.RemoveListener(onGameStateUpdated);
        }

        //----------------- EventSystem notification listeners -----------------

        public void onGameStateUpdated(GameStateUpdateData updateData)
        {
            if (updateData is MapUpdateData)
            {
                updateMap(((MapUpdateData)updateData).GameMap);
            }
            else if (updateData is MapVisibilityUpdateData)
            {
                updateFog(((MapVisibilityUpdateData)updateData).GameMap);
            }
            else if (updateData is ActorUpdateData)
            {
                var a = ((ActorUpdateData)updateData).Actor;
                if (!(a is Player))
                    return;

                updatePlayer((Player)a);
            }
            else if (updateData is ContainerUpdateData)
            {
                var c = ((ContainerUpdateData)updateData).Container;
                if (!(c is GameMap))
                    return;

                updateItems((GameMap)c);
            }
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
            EventManager.UIRequestEvent.Invoke(new MapTileInfoRequest(tilePos));
        }


        // -----------------------------------------------------


        private void updatePlayer(Player playerData)
        {
            DebugUtils.Log("MainViewBehaviour.updatePlayer()");

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
        }


        private void updateMap(GameMap gameMap)
        {
            DebugUtils.Log($"MainViewBehaviour.updateMap(); mapName: {gameMap.Name}");

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

            updateSites(gameMap);
            updateFog(gameMap);
            updateItems(gameMap);
        }


        private void updateSites(GameMap gameMap)
        {
            DebugUtils.Log("MainViewBehaviour.updateSites()");

            UnityUtils.RemoveAllChildren(sitesLayer);
            foreach (var e in gameMap.GetAllEntities<Site>())
            {
                var newEntityObj = Instantiate(entityTemplate, new Vector3(e.x, e.y), Quaternion.identity);
                newEntityObj.name = e.Name; //FIXME: there is no guarantee that entity name is unique
                newEntityObj.GetComponent<SpriteRenderer>().sprite = SpriteCache.Instance.GetSprite("site");
                newEntityObj.transform.SetParent(sitesLayer);
            }
        }

        private void updateItems(GameMap gameMap)
        {
            DebugUtils.Log("MainViewBehaviour.updateItems()");

            UnityUtils.RemoveAllChildren(itemsLayer);
            foreach (var e in gameMap.GetAllEntities<GameItem>())
            {
                var newEntityObj = Instantiate(entityTemplate, new Vector3(e.x, e.y), Quaternion.identity);
                newEntityObj.name = e.Name;
                newEntityObj.GetComponent<SpriteRenderer>().sprite = SpriteCache.Instance.GetSprite("item");
                newEntityObj.transform.SetParent(itemsLayer);
                //TODO: hide site if there is one behind
            }
        }


        private void updateFog(GameMap gameMap)
        {
            DebugUtils.Log("MainViewBehaviour.updateFog()");

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

