
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

    public class TileInfoUpdateEvent : UnityEvent<string, string> { }

    //

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

    //
    public class SystemCommandEvent : UnityEvent<SystemCommand> { }

    public enum SystemCommand
    {
        New,
        Exit,
        Load,
        Save,
    }

    public class ActionRequestEvent : UnityEvent<ActionData> { }

    //
    public class UIRequestEvent : UnityEvent<UIRequestData> { }

    public class UIRequestData { }

    public class MapTilePointerRequest : UIRequestData
    {
        protected Vector2Int? _tilePos;
        public Vector2Int? TilePos { get => _tilePos; }

        public MapTilePointerRequest(Vector2Int? tilePos)
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

    public class ResetViewRequest : UIRequestData { }

    //


    public class EventManager
    {
        public static LocationChangeEvent LocationChangeEvent = new();
        public static MapUpdateEvent MapUpdateEvent = new();
        public static ActorUpdateEvent ActorUpdateEvent = new();
        public static TileInfoUpdateEvent TileInfoUpdateEvent = new();

        public static ContainerUpdateEvent ContainerUpdateEvent = new();
        public static SkillsUpdateEvent SkillsUpdateEvent = new();

        public static StatusNotificationEvent StatusNotificationEvent = new();

        public static SystemCommandEvent SystemCommandEvent = new();
        public static ActionRequestEvent ActionRequestEvent = new();
        public static UIRequestEvent UIRequestEvent = new();
    }
}
