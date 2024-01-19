using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Entities;
using Ventura.Unity.Events;
using Ventura.Unity.ScriptableObjects;
using Ventura.Util;


namespace Ventura.Unity.Behaviours
{

    public class MainViewBehaviour : MonoBehaviour
    {
        //public TerrainColorConfig mapColors;
        public StringColorConfig uiColors;
        public StringSpriteConfig entitySpriteConfig;
        public TerrainSpriteConfig mapSpriteConfig;

        public GameObject terrainTileTemplate;
        public GameObject fogTileTemplate;
        public GameObject entityTemplate;

        [Range(0.0f, 1.0f)]
        public float fogExploredAlpha = 0.2f;

        [Range(0.0f, 1.0f)]
        public float fogUnexploredAlpha = 1.0f;


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

        private GameObject[,] _mapTiles;
        private GameObject[,] _fogTiles;
        private Dictionary<Guid, GameObject> _entityObjs = new();


        private void Awake()
        {
            _entityLayers = new() {
                { typeof(Site), sitesLayer },
                { typeof(GameItem), itemsLayer },
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
            //DebugUtils.Log($"MainViewBehaviour.onGameStateUpdated()");

            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Terrain))
                updateMap(updateData.gameMap);

            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Visibility))
                updateVisibility(updateData.gameMap);

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

            //DebugUtils.Log($"MainViewBehaviour.onEntityUpdated(); update type: [{DataUtils.EnumToStr(updateData.type)}] entity name: [{updateData.entity.Name}]");

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
            //DebugUtils.Log($"MainViewBehaviour.updateMap(); mapName: {gameMap.Name}");

            UnityUtils.RemoveAllChildren(terrainLayer);
            UnityUtils.RemoveAllChildren(fogLayer);

            _mapTiles = new GameObject[gameMap.Width, gameMap.Height];
            _fogTiles = new GameObject[gameMap.Width, gameMap.Height];

            for (int x = 0; x < gameMap.Width; x++)
            {
                for (int y = 0; y < gameMap.Height; y++)
                {
                    TerrainDef terrain = gameMap.Terrain[x, y];

                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newMapTile.GetComponent<MapTileBehaviour>().mapManager = this;
                    newMapTile.GetComponent<MapTileBehaviour>().MapPos = new Vector2Int(x, y);

                    newMapTile.transform.Find("Background").GetComponent<SpriteRenderer>().color = uiColors.Get("mapBackground");
                    newMapTile.transform.Find("Terrain").GetComponent<SpriteRenderer>().color = uiColors.Get("mapTerrains");
                    newMapTile.transform.Find("Terrain").GetComponent<SpriteRenderer>().sprite = mapSpriteConfig.Get(terrain.Type);
                    if (mapSpriteConfig.Get(terrain.Type) == null)
                        throw new GameException($"Sprite not found for terrain {DataUtils.EnumToStr(terrain.Type)}");
                    newMapTile.transform.SetParent(terrainLayer);
                    _mapTiles[x, y] = newMapTile;

                    var newFogTile = Instantiate(fogTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newFogTile.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = uiColors.Get("mapFog");
                    newFogTile.transform.Find("Background").GetComponent<SpriteRenderer>().color = uiColors.Get("mapBackground");
                    newFogTile.transform.SetParent(fogLayer);

                    _fogTiles[x, y] = newFogTile;
                }
            }

            resetEntities(gameMap.GetAllEntities<Site>(), true);
        }


        private void resetEntities<T>(IEnumerable<T> newEntities, bool removeOld) where T : Entity
        {
            //DebugUtils.Log($"MainViewBehaviour.resetEntities() [{typeof(T)}]");

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
            //DebugUtils.Log($"createEntityObj; name: [{e.Name}], eObj.id=[{e.Id}]");

            var newEntityObj = Instantiate(entityTemplate, new Vector3(e.x, e.y), Quaternion.identity);
            newEntityObj.name = e.Name;

            var entitySprite = newEntityObj.transform.Find("Sprite");
            var auraSprite = newEntityObj.transform.Find("Aura");
            var borderSprite = newEntityObj.transform.Find("Border");
            entitySprite.GetComponent<SpriteRenderer>().sprite = entitySpriteConfig.Get(e.SpriteId);
            entitySprite.GetComponent<SpriteRenderer>().color = uiColors.Get("mapEntities");

            if (e is Player)
            {
                borderSprite.GetComponent<SpriteRenderer>().color = uiColors.Get("playerHighlight");
                borderSprite.gameObject.SetActive(true);
            }
            else if (e is GameItem)
            {
                entitySprite.localScale = new Vector3(0.75f, 0.75f, 1.0f);
            }
            else if (e is Monster)
            {
                auraSprite.GetComponent<SpriteRenderer>().color = uiColors.Get("auraHostile");
                auraSprite.gameObject.SetActive(true);
            }

            //var c = UnityUtils.ColorFromHex(e.Color);
            //if (c != null)
            //    newEntityObj.GetComponent<SpriteRenderer>().color = (Color)c;

            newEntityObj.transform.SetParent(_entityLayers[e.GetType()]);
            _entityObjs[e.Id] = newEntityObj;

            resetVisibleEntities(e.x, e.y);

            return newEntityObj;
        }

        private void destroyEntityObj(Entity e)
        {
            //DebugUtils.Log($"destroyEntityObj; name: [{e.Name}], eObj.id=[{e.Id}]");
            destroyEntityObj(e.Id);
        }

        private void destroyEntityObj(Guid entityId)
        {
            //DebugUtils.Log($"destroyEntityObj; e.id=[{entityId}]");

            if (_entityObjs.ContainsKey(entityId))
            {
                var targetEntityObj = _entityObjs[entityId];

                var x = (int)targetEntityObj.transform.position.x;
                var y = (int)targetEntityObj.transform.position.y;

                DestroyImmediate(targetEntityObj);
                _entityObjs.Remove(entityId);
                resetVisibleEntities(x, y);
            }
        }

        private GameObject updateEntityObj(Entity e)
        {
            //DebugUtils.Log($"updateEntityObj; name: [{e.Name}], id=[{e.Id}]");
            if (!_entityObjs.ContainsKey(e.Id))
            {
                //DebugUtils.Log($"entity with id=[{e.Id}] not found in _entityObjs, skipping update");
                return null;
            }

            var entityObj = _entityObjs[e.Id];

            var oldX = (int)entityObj.transform.position.x;
            var oldY = (int)entityObj.transform.position.y;
            var newPos = new Vector3(e.x, e.y);

            entityObj.transform.position = newPos;

            resetVisibleEntities(oldX, oldY);
            resetVisibleEntities(e.x, e.y);

            return entityObj;
        }

        private void resetVisibleEntities(int x, int y)
        {
            // TODO: make more efficient, either with a data structure or using Unity (colliders?)
            var targetEntityObjs = new List<GameObject>();
            //DebugUtils.Log($"resetVisibleEntities x={x}, y={y}");

            foreach (var eId in _entityObjs.Keys)
            {
                var eObj = _entityObjs[eId];
                if (eObj == null)
                    throw new GameException($"Inconsistent status for entityObj with id [{eId}]");

                if ((eObj.transform.position.x == x) && (eObj.transform.position.y == y))
                    targetEntityObjs.Add(eObj);
            }

            if (targetEntityObjs.Count == 0)
                return;


            bool isTileVisible = false;
            if (_mapTiles != null && x >= 0 && x < _mapTiles.GetLength(0) && y >= 0 && y < _mapTiles.GetLength(1))
                isTileVisible = _mapTiles[x, y].GetComponent<MapTileBehaviour>().isVisible;

            if (targetEntityObjs.Count == 1)
            {
                //targetEntityObjs[0].transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = isTileVisible;
                targetEntityObjs[0].SetActive(isTileVisible);
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
                    mustEnable = isTileVisible;
                else
                    mustEnable = false;

                //e.transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = mustEnable;
                e.SetActive(mustEnable);
            }
        }

        private void updateVisibility(GameMap gameMap)
        {
            //DebugUtils.Log("MainViewBehaviour.updateVisibility()");
            if ((_fogTiles == null) || (_fogTiles.GetLength(0) != gameMap.Width) || (_fogTiles.GetLength(1) != gameMap.Height))
            {
                DebugUtils.Warning("visibility data is (still) inconsistent with our terrain, ignoring it");
                return;
            }

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

                    var fogSpriteColor = _fogTiles[x, y].transform.Find("Sprite").GetComponent<SpriteRenderer>().color;
                    fogSpriteColor.a = alpha;
                    _fogTiles[x, y].transform.Find("Sprite").GetComponent<SpriteRenderer>().color = fogSpriteColor;

                    var fogBackgroundColor = _fogTiles[x, y].transform.Find("Background").GetComponent<SpriteRenderer>().color;
                    fogBackgroundColor.a = alpha;
                    _fogTiles[x, y].transform.Find("Background").GetComponent<SpriteRenderer>().color = fogBackgroundColor;

                    //surreptitiously using mapTile to store visibility status
                    _mapTiles[x, y].GetComponent<MapTileBehaviour>().isVisible = gameMap.Visible[x, y];
                    resetVisibleEntities(x, y);
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
            //DebugUtils.Log($"MainViewBehaviour.updateCamera()");

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

