using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;

namespace Ventura.Unity.Events
{
    //------------- GameState Updates ------------
    public class GameStateUpdateEvent : UnityEvent<GameStateUpdateData> { }

    public class GameStateUpdateData { }


    public class LocationUpdateData : GameStateUpdateData
    {
        private ReadOnlyCollection<string> _mapStackNames;
        public ReadOnlyCollection<string> MapStackNames { get => _mapStackNames; }

        public LocationUpdateData(ReadOnlyCollection<string> mapStackNames)
        {
            _mapStackNames = mapStackNames;
        }
    }

    public class MapUpdateData : GameStateUpdateData
    {
        private GameMap _gameMap;
        public GameMap GameMap { get => _gameMap; }

        public MapUpdateData(GameMap gameMap)
        {
            _gameMap = gameMap;
        }
    }

    public class MapVisibilityUpdateData : GameStateUpdateData
    {
        //FIXME: expose only relevant fields
        private GameMap _gameMap;
        public GameMap GameMap { get => _gameMap; }

        public MapVisibilityUpdateData(GameMap gameMap)
        {
            _gameMap = gameMap;
        }
    }

    public class ActorUpdateData : GameStateUpdateData
    {
        private Actor _actor;
        public Actor Actor { get => _actor; }

        public ActorUpdateData(Actor actor)
        {
            _actor = actor;
        }
    }

    public class SkillsUpdateData : GameStateUpdateData
    {
        private Skills _skills;
        public Skills Skills { get => _skills; }

        public SkillsUpdateData(Skills skills)
        {
            _skills = skills;
        }
    }

    public class ContainerUpdateData : GameStateUpdateData
    {
        private Container _container;
        public Container Container { get => _container; }

        public ContainerUpdateData(Container gameMap)
        {
            _container = gameMap;
        }
    }

    public class TileUpdateData : GameStateUpdateData
    {
        private Vector2Int _pos;
        public Vector2Int Pos { get => _pos; set => _pos = value; }

        private string _terrain;
        public string Terrain { get => _terrain; set => _terrain = value; }

        private string _site;
        public string Site { get => _site; set => _site = value; }

        private string _actor;
        public string Actor { get => _actor; set => _actor = value; }

        private ReadOnlyCollection<string> _items;
        public ReadOnlyCollection<string> Items { get => _items; set => _items = value; }
    }


    //------------- Status Line Notifications ------------

    public class StatusNotificationEvent : UnityEvent<string, StatusSeverity>
    {
        public void Invoke(string msg)
        {
            if (msg == null)
                msg = "";
            Invoke(msg, StatusSeverity.Normal);
        }
    }

    public enum StatusSeverity
    {
        Normal,
        Warning,
        Critical,
    }

    //------------- System Commands ------------

    public class SystemCommandEvent : UnityEvent<SystemCommand> { }

    public enum SystemCommand
    {
        New,
        Exit,
        Load,
        Save,
    }

    //------------- Actions ------------

    public class ActionRequestEvent : UnityEvent<ActionData> { }

    //

    //------------- UI Requests ------------

    public class UIRequestEvent : UnityEvent<UIRequestData> { }

    public class UIRequestData { }

    public class ResetViewRequest : UIRequestData { }


    public class MapTileInfoRequest : UIRequestData
    {
        protected Vector2Int? _tilePos;
        public Vector2Int? TilePos { get => _tilePos; }

        public MapTileInfoRequest(Vector2Int? tilePos)
        {
            _tilePos = tilePos;
        }
    }


    public class AskConfirmationRequest : UIRequestData
    {
        protected string _title;
        public string Title { get => _title; }

        protected SystemCommand _command;
        public SystemCommand Command { get => _command; }

        public AskConfirmationRequest(string title, SystemCommand command)
        {
            _title = title;
            _command = command;
        }
    }

    //


    public class EventManager
    {
        public static GameStateUpdateEvent GameStateUpdateEvent = new();
        public static StatusNotificationEvent StatusNotificationEvent = new();

        public static SystemCommandEvent SystemCommandEvent = new();
        public static ActionRequestEvent ActionRequestEvent = new();
        public static UIRequestEvent UIRequestEvent = new();
    }
}
