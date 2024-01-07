using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Entities;
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


        public GameObject cameraObj;
        public Transform terrainLayer;
        public Transform sitesLayer;
        public Transform itemsLayer;
        public Transform monstersLayer;
        public Transform fogLayer;
        public Transform playerLayer;


        public GameObject pathFinderLineObj;

        private Dictionary<Type, Transform> _entityLayers;
        private Dictionary<Transform, int> _layerPriorities;

        private GameObject[,] _fogTiles;
        private Dictionary<Guid, GameObject> _entityObjs = new();


        private void Awake()
        {
            _entityLayers = new() {
                { typeof(Site), sitesLayer },
                { typeof(GameItem), itemsLayer },
                { typeof(BookItem), itemsLayer }, //FIXME
                { typeof(Monster), monstersLayer },
                { typeof(Player), playerLayer },
            };

            //bigger is overlayed over smaller
            _layerPriorities = new()
            {
                { itemsLayer, 1 },
                { sitesLayer, 2 },
                { monstersLayer, 3 },
                { playerLayer, 4 },
            };
        }

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
                resetEntities(updateData.gameMap.GetAllEntities<GameItem>(), true);

            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Actors))
            {
                resetEntities(updateData.gameMap.GetAllEntities<Monster>(), true);
                resetEntities(updateData.gameMap.GetAllEntities<Player>(), true);
                updateCamera();
            }

        }

        private void onEntityUpdated(EntityUpdate updateData)
        {
            var a = updateData.entity;

            DebugUtils.Log("MainViewBehaviour.onEntityUpdated()");

            if (updateData.type == EntityUpdate.Type.Added)
            {
                createEntityObj(updateData.entity);
            }
            else if (updateData.type == EntityUpdate.Type.Removed)
            {
                destroyEntityObj(updateData.entity);
            }
            else if (updateData.type == EntityUpdate.Type.Changed)
            {
                updateEntityObj(updateData.entity);
            }

            if (a is Player)
                updateCamera();
        }

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

            resetEntities(gameMap.GetAllEntities<Site>(), true);
        }


        private void resetEntities<T>(IEnumerable<T> newEntities, bool removeOld) where T : Entity
        {
            //DebugUtils.Log("MainViewBehaviour.resetEntities()");

            //find all old entityObjs on the correct layer
            var toBeRemoved = new HashSet<Guid>();
            foreach (var entityId in _entityObjs.Keys)
            {
                if (_entityObjs[entityId].transform.parent == _entityLayers[typeof(T)])
                    toBeRemoved.Add(entityId);
            }

            //add new entityObjs
            foreach (var newEntity in newEntities)
            {
                var newEntityObj = updateEntityObj(newEntity);
                if (newEntityObj == null)
                    newEntityObj = createEntityObj(newEntity);

                toBeRemoved.Remove(newEntity.Id);
            }

            //remove old entityObjs
            foreach (var entityId in toBeRemoved)
            {
                destroyEntityObj(entityId);
            }
        }

        private GameObject createEntityObj(Entity e)
        {
            //DebugUtils.Log($"createEntityObj; name: [{e.Name}], e.id=[{e.Id}]");
            var pos = new Vector3(e.x, e.y);

            var newEntityObj = Instantiate(entityTemplate, pos, Quaternion.identity);
            newEntityObj.name = e.Name;

            string spriteId = null;

            //TODO: make generic
            if (e is Site)
                spriteId = "site";
            else if ((e is GameItem) || (e.GetType().IsSubclassOf(typeof(GameItem))))
                spriteId = "item";
            else if (e is Monster)
                spriteId = "butterfly";
            else if (e is Player)
                spriteId = "player";
            //

            newEntityObj.GetComponent<SpriteRenderer>().sprite = SpriteCache.Instance.GetSprite(spriteId);

            //TODO: set color for butterflies
            //newEntityObj.GetComponent<SpriteRenderer>().color = UnityUtils.ColorFromHash(e.GetHashCode());

            newEntityObj.transform.SetParent(_entityLayers[e.GetType()]);
            _entityObjs[e.Id] = newEntityObj;

            resetVisibleEntities(pos);

            return newEntityObj;
        }

        private void destroyEntityObj(Entity e)
        {
            //DebugUtils.Log($"destroyEntityObj; name: [{e.Name}], e.id=[{e.Id}]");
            destroyEntityObj(e.Id);
        }

        private void destroyEntityObj(Guid entityId)
        {
            //DebugUtils.Log($"destroyEntityObj; e.id=[{entityId}]");
            if (_entityObjs.ContainsKey(entityId))
            {
                var targetEntityObj = _entityObjs[entityId];
                var pos = targetEntityObj.transform.position;
                Destroy(targetEntityObj);
                _entityObjs.Remove(entityId);

                resetVisibleEntities(pos);
            }
        }

        private GameObject updateEntityObj(Entity e)
        {
            //DebugUtils.Log($"updateEntityObj; name: [{e.Name}], id=[{e.Id}]");
            if (!_entityObjs.ContainsKey(e.Id))
                return null;

            //DebugUtils.Log($"updateEntityObj ok");

            var entityObj = _entityObjs[e.Id];
            var oldPos = entityObj.transform.position;
            var newPos = new Vector3(e.x, e.y);

            entityObj.transform.position = newPos;

            resetVisibleEntities(oldPos);
            resetVisibleEntities(newPos);

            return entityObj;
        }

        private void resetVisibleEntities(Vector3 pos)
        {
            ////TODO: make more efficient, either with a data structure or using Unity (colliders?)
            var targetEntityObjs = new List<GameObject>();

            foreach (var e in _entityObjs.Values)
            {
                if ((e.transform.position.x == pos.x) && (e.transform.position.y == pos.y))
                    targetEntityObjs.Add(e);
            }

            if (targetEntityObjs.Count == 0)
                return;

            if (targetEntityObjs.Count == 1)
            {
                targetEntityObjs[0].GetComponent<SpriteRenderer>().enabled = true;
                return;
            }

            targetEntityObjs.Sort(delegate (GameObject eObj1, GameObject eObj2)
            {
                var pr1 = _layerPriorities[eObj1.transform.parent];
                var pr2 = _layerPriorities[eObj2.transform.parent];

                return pr1.CompareTo(pr2);
            });


            foreach (var e in targetEntityObjs)
            {
                //last element has highest prority
                bool mustEnable;
                if (e == targetEntityObjs[^1])
                    mustEnable = true;
                else
                    mustEnable = false;

                e.GetComponent<SpriteRenderer>().enabled = mustEnable;
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

        private void updateCamera()
        {
            DebugUtils.Log("MainViewBehaviour.updateCamera()");

            if (playerLayer.childCount == 0)
                return;

            var playerPos = playerLayer.GetChild(0).position;
            var newCameraPos = cameraObj.transform.position;
            newCameraPos.x = playerPos.x;
            newCameraPos.y = playerPos.y;
            cameraObj.transform.position = newCameraPos;
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

