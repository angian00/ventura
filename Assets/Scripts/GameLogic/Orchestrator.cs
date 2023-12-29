﻿
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Ventura.GameLogic.Actions;
using Ventura.Generators;
using Ventura.Util;


namespace Ventura.GameLogic
{
    public class Orchestrator
    {
        private static Orchestrator _instance = new Orchestrator();
        public static Orchestrator GetInstance()
        {
            return _instance;
        }

        private const int VISIBILITY_RADIUS = 4;


        private GameMap _currMap;
        public GameMap CurrMap { get => _currMap; set => _currMap = value; }

        private World _world;
        public World World { get => _world; }

        private Actor _player;
        public Actor Player { get => _player; }

        private SortedSet<PendingType> _pendingUpdates = new();
        public SortedSet<PendingType> PendingUpdates { get => _pendingUpdates; }

        //private HashSet<string> _exploredMaps = new();
        //private List<Actor> _actors;
        //private bool isGameActive = false;

        private CircularList<Actor> _scheduler = new();
        private Queue<GameAction> _playerActionQueue = new();


        public enum PendingType
        {
            Terrain,
            Player,
        }


        public Orchestrator()
        {
            _world = new World();
        }

        public void NewGame()
        {
            Messages.Log("Orchestrator.NewGame()");

            //await loadAllData()
            //this.player = actorDefs["player"].clone()
            _player = new Actor(this, "player");


            const int WORLD_MAP_WIDTH = 80;
            const int WORLD_MAP_HEIGHT = 50;

            //var startMap = mapDefs["test_map_world"];
            var startMap = MapGenerator.GenerateWildernessMap(WORLD_MAP_WIDTH, WORLD_MAP_HEIGHT);
            _world.PushMap(startMap, null);
            _currMap = startMap;
            _currMap.Entities.Add(_player);

            MoveActorTo(_player, _currMap.StartingPos.x, _currMap.StartingPos.y);

            _currMap.UpdateExploration(_player.x, _player.y, VISIBILITY_RADIUS);

            ActivateActors();

            /*
            //DEBUG: add a consumable item
            let potion = makeItem(this, ItemType.PotionHealth)
            this.map.place(potion, 16, 8)
            //

            //DEBUG: add equipment items
            let dagger = makeItem(this, ItemType.Dagger)
            this.map.place(dagger, 19, 8)

            let armor = makeItem(this, ItemType.LeatherArmor)
            this.map.place(armor, 19, 10)
            //

            //DEBUG: add ingredients
            let herb1 = makeItem(this, ItemType.HerbHenbane)
            this.map.place(herb1, 17, 8)

            let herb2 = makeItem(this, ItemType.HerbNightshade)
            this.map.place(herb2, 17, 10)
            //
        */
            //fov.compute(this.player.x, this.player.y, lightRadius, this.setFov.bind(this))

            Messages.Display("Welcome, adventurer!");

            _pendingUpdates.Add(PendingType.Terrain);
            _pendingUpdates.Add(PendingType.Player);
        }


        public bool IsTransparent(int x, int y)
        {
            if (_currMap == null)
                return false;

            if (x < 0 || x >= _currMap.Width || y < 0 || y >= _currMap.Height)
                return false;

            else
                return _currMap.Terrain[x, y].Transparent;
        }


        public bool IsWalkable(int x, int y)
        {
            if (_currMap == null)
                return false;

            if (x < 0 || x >= _currMap.Width || y < 0 || y >= _currMap.Height)
                return false;

            else
                return (_currMap.Terrain[x, y].Walkable) && (_currMap.GetBlockingEntityAt(x, y) == null);
        }


        public Vector2Int MoveActorTo(Actor a, int targetX, int targetY)
        {
            if (IsWalkable(targetX, targetY))
            {

                a.MoveTo(targetX, targetY);
                if (a.Name == "player") //FIXME: use a more robust way to check if actor is player
                    _currMap.UpdateExploration(targetX, targetY, VISIBILITY_RADIUS);

            }

            _pendingUpdates.Add(PendingType.Player);

            return new Vector2Int(a.x, a.y);
        }

        public void MoveItemTo(GameItem item, Container targetContainer)
        {
            _currMap.RemoveItem(item);
            targetContainer.AddItem(item);
            item.Parent = targetContainer;
        }


        public void EnqueuePlayerAction(GameAction a)
        {
            //Messages.Log("EnqueuePlayerAction");
            _playerActionQueue.Enqueue(a);
        }


        public GameAction? DequeuePlayerAction()
        {
            if (_playerActionQueue.Count > 0)
                return _playerActionQueue.Dequeue();

            return null;
        }


        public void ActivateActors() {
            _scheduler.Add(_player);

            if (_currMap == null)
                return;

            foreach (var a in _currMap.Entities) {
                if (a is Actor && a.Name != "player")
                    _scheduler.Add((Actor)a);
		    }
        }

        public void DeactivateActors() {
            _scheduler.Clear();
        }
        
        public void ProcessTurn()
        {
            var currActor = _scheduler.Next();
            if (currActor != null)
                currActor.Act();
        }


        public void EnterMap(string mapName)
        {
            DeactivateActors();
            _currMap.Entities.Remove(_player);

            //if (!(target.mapName in mapDefs))
            //    return new ActionResult(false, $"Unknown map: [{TargetSite.mapName}]");
            //_orch.World.PushMap(mapDefs[target.mapName]);

            var newMap = MapGenerator.GenerateWildernessMap(10, 10, mapName, false);
            //newMap.Orchestrator = this;

            _world.PushMap(newMap, new Vector2Int(_player.x, _player.y));
            _currMap = newMap;
            //_exploredMaps.add(newMap.name);

            _currMap.Entities.Add(_player);
            _pendingUpdates.Add(PendingType.Terrain);

            var startPos = _currMap.StartingPos;
            Messages.Log($"EnterMap; startPos={startPos}");
            if (startPos == null)
                startPos = (Vector2Int)_currMap.GetRandomWalkablePos();

            MoveActorTo(_player, startPos.x, startPos.y);

            ActivateActors();
        }

        public void ExitMap()
        {
            DeactivateActors();
            _currMap.Entities.Remove(_player);

            var previousMapPos = _world.PopMap();
            _currMap = _world.CurrMap;

            _currMap.Entities.Add(_player);
            _pendingUpdates.Add(PendingType.Terrain);

            MoveActorTo(_player, previousMapPos.x, previousMapPos.y);

            ActivateActors();
        }
    }
}
