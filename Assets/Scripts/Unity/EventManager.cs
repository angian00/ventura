
using System.Collections.ObjectModel;
using UnityEngine.Events;
using Ventura.GameLogic;
using Ventura.GameLogic.Components;
using Ventura.Unity.Behaviours;

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
    }
}
