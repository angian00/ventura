
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;

namespace Ventura.Unity.Events
{
    public class LocationChangeEvent : UnityEvent<GameMap, ReadOnlyCollection<string>> { }
    public class MapUpdateEvent : UnityEvent<GameMap> { }
    public class ActorUpdateEvent : UnityEvent<Actor> { }

    public class SkillsUpdateEvent : UnityEvent<Skills> { }
    public class ContainerUpdateEvent : UnityEvent<Container> { }

    public class MapInfoUpdateEvent : UnityEvent<string, string> { }

    //

    public class StatusNotificationEvent : UnityEvent<string, StatusSeverity> {
        public void Invoke(string msg) {
            Invoke(msg, StatusSeverity.Normal);
        }

        public void Invoke()
        {
            Invoke("", StatusSeverity.Normal);
        }
    }

    public enum StatusSeverity
    {
        Normal,
        Warning,
        Critical,
    }

    //
    public class SystemCommandRequestEvent : UnityEvent<SystemCommand> { }

    public enum SystemCommand
    {
        New,
        Exit,
        Load,
        Save,
    }

    //
    //public class ActionRequestEvent : UnityEvent<PlayerActionRequest> { }

    //public class PlayerActionRequest { }
    //public class UseItemActionRequest: PlayerActionRequest
    //{
    //    protected GameItem _gameItem;
    //    public GameItem GameItem { get => _gameItem; }

    //    public UseItemActionRequest(GameItem gameItem)
    //    {
    //        _gameItem = gameItem;
    //    }
    //}

    public class ActionRequestEvent : UnityEvent<ActionData> { }

    //
    public class UIRequestEvent : UnityEvent<UIRequest> { }

    public class UIRequest { }

    public class MapTileInfoRequest: UIRequest {
        protected Vector2Int? _tilePos;
        public Vector2Int? TilePos { get => _tilePos; }

        public MapTileInfoRequest(Vector2Int? tilePos)
        {
            _tilePos = tilePos;
        }
    }

    public class ViewResetRequest : UIRequest { }

    //


    public class EventManager
    {
        public static LocationChangeEvent LocationChangeEvent = new();
        public static MapUpdateEvent MapUpdateEvent = new();
        public static ActorUpdateEvent ActorUpdateEvent = new();

        public static ContainerUpdateEvent ContainerUpdateEvent = new();
        public static SkillsUpdateEvent SkillsUpdateEvent = new();

        public static MapInfoUpdateEvent MapInfoUpdateEvent = new();

        public static StatusNotificationEvent StatusNotificationEvent = new();

        public static SystemCommandRequestEvent SystemCommandRequestEvent = new();
        public static ActionRequestEvent ActionRequestEvent = new();
        public static UIRequestEvent UIRequestEvent = new();

    }
}
