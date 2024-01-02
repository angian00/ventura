
using System.Collections.ObjectModel;
using UnityEngine.Events;
using Ventura.GameLogic;
using Ventura.GameLogic.Components;

namespace Ventura.Unity.Events
{
    public class MapChangeEvent : UnityEvent<GameMap, ReadOnlyCollection<string>> { }
    public class MapUpdateEvent : UnityEvent<GameMap> { }
    public class ActorUpdateEvent : UnityEvent<Actor> { }

    public class SkillsUpdateEvent : UnityEvent<Skills> { }
    public class ContainerUpdateEvent : UnityEvent<Container> { }


    public class EventManager
    {
        public static MapChangeEvent MapChangeEvent = new();
        public static MapUpdateEvent MapUpdateEvent = new();
        public static ActorUpdateEvent ActorUpdateEvent = new();

        public static ContainerUpdateEvent ContainerUpdateEvent = new();
        public static SkillsUpdateEvent SkillsUpdateEvent = new();
    }
}
