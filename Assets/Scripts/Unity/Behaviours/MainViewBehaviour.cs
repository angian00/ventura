using System.Collections.Generic;
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


        private float[] zoomLevelsFactors = { 0.5f, 0.75f, 1.0f, 1.5f, 2.0f, };
        private int _zoomLevelIndex = 2;
        private const int defaultZoomLevel = 10;


        public GameObject playerObj;
        public GameObject cameraObj;
        public Transform terrainLayer;
        public Transform sitesLayer;
        public Transform itemsLayer;
        public Transform monstersLayer;
        public Transform fogLayer;

        public GameObject pathFinderLineObj;

        private GameObject[,] _fogTiles;


        private void OnEnable()
        {
            EventManager.Subscribe<GameStateUpdate>(onGameStateUpdated);
            EventManager.Subscribe<EntityUpdate>(onEntityUpdated);
            EventManager.Subscribe<UIRequest>(onUIRequest);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<GameStateUpdate>(onGameStateUpdated);
            EventManager.Unsubscribe<EntityUpdate>(onEntityUpdated);
            EventManager.Unsubscribe<UIRequest>(onUIRequest);
        }

        //----------------- EventSystem notification listeners -----------------

        private void onGameStateUpdated(GameStateUpdate updateData)
        {
            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Terrain))
                updateMap(updateData.gameMap);

            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Visibility))
                updateFog(updateData.gameMap);

            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Items))
                updateItems(updateData.gameMap.GetVisibleEntities<GameItem>());

            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Monsters))
                updateMonsters(updateData.gameMap.GetVisibleEntities<Monster>());

        }

        private void onEntityUpdated(EntityUpdate updateData)
        {
            var a = updateData.entity;
            if (a is Player)
                updatePlayer((Player)a);
        }

        //private void onItemsUpdated(EntityNotification updateData)
        //{ 
        //}

        //private void onInfoUpdated(EntityNotification updateData)
        //{
        //    var path = ((PathfindingUpdateData)updateData).Path;
        //    drawLine(path);
        //}

        public void onUIRequest(UIRequest uiRequest)
        {
            if (uiRequest.command == UIRequest.Command.ZoomIn)
                updateZoomLevel(true);
            else if (uiRequest.command == UIRequest.Command.ZoomOut)
                updateZoomLevel(false);
        }

        // ------ mouse input handlers -------------------------
        public void OnTileClick(Vector2Int tilePos)
        {
            DebugUtils.Log("MainView.OnTileClick");
            //EventManager.UIRequestEvent.Invoke(new PathfindingRequest(tilePos)); //FUTURE: reinsert pathfinding on click
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
            EventManager.Publish(new TileInfoRequest(tilePos));
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
            //updateFog(gameMap);
            //updateItems(gameMap);
            //updateMonsters(gameMap);
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

        private void updateItems(IEnumerable<GameItem> items)
        {
            DebugUtils.Log("MainViewBehaviour.updateItems()");

            UnityUtils.RemoveAllChildren(itemsLayer);
            foreach (var item in items)
            {
                var newEntityObj = Instantiate(entityTemplate, new Vector3(item.x, item.y), Quaternion.identity);
                newEntityObj.name = item.Name;
                newEntityObj.GetComponent<SpriteRenderer>().sprite = SpriteCache.Instance.GetSprite("item");
                newEntityObj.transform.SetParent(itemsLayer);
                //TODO: hide site if there is one behind
            }
        }

        private void updateMonsters(IEnumerable<Monster> monsters)
        {
            DebugUtils.Log("MainViewBehaviour.updateMonsters()");

            UnityUtils.RemoveAllChildren(monstersLayer);
            foreach (var m in monsters)
            {
                var newEntityObj = Instantiate(entityTemplate, new Vector3(m.x, m.y), Quaternion.identity);
                newEntityObj.name = m.Name;
                newEntityObj.GetComponent<SpriteRenderer>().sprite = SpriteCache.Instance.GetSprite("butterfly"); //TODO: make generic
                newEntityObj.GetComponent<SpriteRenderer>().color = UnityUtils.ColorFromHash(m.GetHashCode()); //TODO: make generic
                newEntityObj.transform.SetParent(monstersLayer);

                //TODO: hide site or item if there is one behind
            }
        }

        private void updateFog(GameMap gameMap)
        {
            if ((_fogTiles == null) || (_fogTiles.GetLength(0) != gameMap.Width) || (_fogTiles.GetLength(1) != gameMap.Height))
            {
                //visibility data is (still) inconsistent with our terrain, ignore
                return;
            }

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


        private void updateZoomLevel(bool zoomIn)
        {
            if (zoomIn)
            {
                if (_zoomLevelIndex > 0)
                    _zoomLevelIndex--;
            }
            else
            {
                if (_zoomLevelIndex < zoomLevelsFactors.Length - 1)
                    _zoomLevelIndex++;
            }

            cameraObj.GetComponent<Camera>().orthographicSize = defaultZoomLevel * zoomLevelsFactors[_zoomLevelIndex];
        }

        // -----------------------------------------------------

        private void drawLine(List<Vector2Int> tilePath)
        {
            //pathFinderLineObj.transform.position = ;

            var lr = pathFinderLineObj.GetComponent<LineRenderer>();

            //lr.SetPosition(0, new Vector3(startPos.x, startPos.y, 1));
            //lr.SetPosition(1, new Vector3(endPos.x, endPos.y, 1));

            var path = new Vector3[tilePath.Count];
            for (int i = 0; i < tilePath.Count; i++)
            {
                var tilePos = tilePath[i];
                path[i] = new Vector3(tilePos.x, tilePos.y, 0);
            }

            lr.positionCount = tilePath.Count;
            lr.SetPositions(path);

            //GameObject.Destroy(pathFinderLineObj, duration);
        }
    }
}

