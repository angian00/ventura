using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Entities;

namespace Ventura.Unity.Events
{
    public abstract record GameEventData { }

    public record GameStateUpdate : GameEventData
    {
        [Flags]
        public enum UpdatedFields
        {
            None = 0,

            Location = 1,
            Terrain = 2,
            Visibility = 4,
            Items = 8,
            Actors = 16,

            Everything = ~0,
        }

        public List<string> mapStackNames { get; }
        public GameMap gameMap { get; }
        public UpdatedFields updatedFields { get; }

        public GameStateUpdate(List<string> mapStackNames, GameMap gameMap, UpdatedFields updatedFields)
        {
            this.mapStackNames = mapStackNames;
            this.gameMap = gameMap;
            this.updatedFields = updatedFields;
        }
    }

    public record EntityUpdate : GameEventData
    {
        public enum Type
        {
            Added,
            Removed,
            Changed,
        }

        public Type type { get; }
        public Entity entity { get; }

        public EntityUpdate(Type type, Entity entity)
        {
            this.type = type;
            this.entity = entity;
        }
    }


    public record TextNotification : GameEventData
    {
        public enum Severity
        {
            Normal,
            Warning,
            Critical,
        }

        public string? msg { get; }
        public Severity severity { get; }

        public TextNotification(string msg, Severity severity)
        {
            this.msg = msg;
            this.severity = severity;
        }

        public TextNotification(string msg) : this(msg, Severity.Normal) { }
    }


    public record SystemRequest : GameEventData
    {
        public enum Command
        {
            New,
            Exit,
            Load,
            Save,
            GameOver,
        }

        public Command command { get; }

        public SystemRequest(Command command)
        {
            this.command = command;
        }
    }

    public record ActionRequest : GameEventData
    {
        public ActionData actionData { get; }

        public ActionRequest(ActionData actionData)
        {
            this.actionData = actionData;
        }
    }

    public enum InfoType
    {
        TileContent,
    }

    public record InfoRequest : GameEventData
    {
        public InfoType infoType { get; }

        public InfoRequest(InfoType infoType)
        {
            this.infoType = infoType;
        }
    }

    public record TileInfoRequest : InfoRequest
    {
        public Vector2Int? pos { get; }

        public TileInfoRequest(Vector2Int? pos) : base(InfoType.TileContent)
        {
            this.pos = pos;
        }
    }

    public record InfoResponse : GameEventData
    {
        public InfoType infoType { get; }

        public InfoResponse(InfoType infoType)
        {
            this.infoType = infoType;
        }
    }

    public record TileInfoResponse : InfoResponse
    {
        public Vector2Int? pos { get; set; }
        public string terrain { get; set; }
        public string site { get; set; }
        public string actor { get; set; }
        public List<string> items { get; set; }

        public TileInfoResponse(Vector2Int pos) : base(InfoType.TileContent)
        {
            this.pos = pos;
        }

        public TileInfoResponse() : base(InfoType.TileContent) { }
    }


    public record UIRequest : GameEventData
    {
        public enum Command
        {
            ResetView,
            ZoomIn,
            ZoomOut,
            AskYesNo,
        }

        public Command command { get; }


        public UIRequest(Command command)
        {
            this.command = command;
        }
    }

    public record AskYesNoRequest : UIRequest
    {
        public string title { get; }
        public SystemRequest.Command systemCommand { get; }

        public AskYesNoRequest(string title, SystemRequest.Command systemCommand) : base(Command.AskYesNo)
        {
            this.title = title;
            this.systemCommand = systemCommand;
        }
    }



    public class EventManager
    {
        private static Action<GameStateUpdate> gameStateUpdateDelegates;
        private static Action<EntityUpdate> entityUpdateDelegates;
        private static Action<TextNotification> textNotificationDelegates;
        private static Action<UIRequest> uiRequestDelegates;
        private static Action<SystemRequest> systemRequestDelegates;
        private static Action<ActionRequest> actionRequestDelegates;
        private static Action<InfoRequest> infoRequestDelegates;
        private static Action<InfoResponse> infoResponseDelegates;

        public static void Publish(GameEventData eventData)
        {
            if (eventData is GameStateUpdate)
                gameStateUpdateDelegates?.Invoke((GameStateUpdate)eventData);
            else if (eventData is EntityUpdate)
                entityUpdateDelegates?.Invoke((EntityUpdate)eventData);
            else if (eventData is TextNotification)
                textNotificationDelegates?.Invoke((TextNotification)eventData);
            else if (eventData is UIRequest)
                uiRequestDelegates?.Invoke((UIRequest)eventData);
            else if (eventData is SystemRequest)
                systemRequestDelegates?.Invoke((SystemRequest)eventData);
            else if (eventData is ActionRequest)
                actionRequestDelegates?.Invoke((ActionRequest)eventData);
            else if (eventData is InfoRequest)
                infoRequestDelegates?.Invoke((InfoRequest)eventData);
            else if (eventData is InfoResponse)
                infoResponseDelegates?.Invoke((InfoResponse)eventData);

            else
                throw new GameException($"Invalid event type: {eventData.GetType()}");
        }



        public static void Subscribe<T>(Action<T> handler) where T : GameEventData
        {
            if (typeof(T) == typeof(GameStateUpdate))
                gameStateUpdateDelegates += (Action<GameStateUpdate>)handler;
            else if (typeof(T) == typeof(EntityUpdate))
                entityUpdateDelegates += (Action<EntityUpdate>)handler;
            else if (typeof(T) == typeof(TextNotification))
                textNotificationDelegates += (Action<TextNotification>)handler;
            else if (typeof(T) == typeof(UIRequest))
                uiRequestDelegates += (Action<UIRequest>)handler;
            else if (typeof(T) == typeof(SystemRequest))
                systemRequestDelegates += (Action<SystemRequest>)handler;
            else if (typeof(T) == typeof(ActionRequest))
                actionRequestDelegates += (Action<ActionRequest>)handler;
            else if (typeof(T) == typeof(InfoRequest))
                infoRequestDelegates += (Action<InfoRequest>)handler;
            else if (typeof(T) == typeof(InfoResponse))
                infoResponseDelegates += (Action<InfoResponse>)handler;

            else
                throw new GameException($"Invalid event type: {typeof(T)}");
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : GameEventData
        {
            if (typeof(T) == typeof(GameStateUpdate))
            {
                if (gameStateUpdateDelegates != null)
                    gameStateUpdateDelegates -= (Action<GameStateUpdate>)handler;
            }
            else if (typeof(T) == typeof(EntityUpdate))
            {
                if (entityUpdateDelegates != null)
                    entityUpdateDelegates -= (Action<EntityUpdate>)handler;
            }
            else if (typeof(T) == typeof(TextNotification))
            {
                if (textNotificationDelegates != null)
                    textNotificationDelegates -= (Action<TextNotification>)handler;
            }
            else if (typeof(T) == typeof(UIRequest))
            {
                if (uiRequestDelegates != null)
                    uiRequestDelegates -= (Action<UIRequest>)handler;
            }
            else if (typeof(T) == typeof(SystemRequest))
            {
                if (systemRequestDelegates != null)
                    systemRequestDelegates -= (Action<SystemRequest>)handler;
            }
            else if (typeof(T) == typeof(ActionRequest))
            {
                if (actionRequestDelegates != null)
                    actionRequestDelegates -= (Action<ActionRequest>)handler;
            }
            else if (typeof(T) == typeof(InfoRequest))
            {
                if (infoRequestDelegates != null)
                    infoRequestDelegates -= (Action<InfoRequest>)handler;
            }
            else if (typeof(T) == typeof(InfoResponse))
            {
                if (infoResponseDelegates != null)
                    infoResponseDelegates -= (Action<InfoResponse>)handler;
            }
            else
                throw new GameException($"Invalid event type: {typeof(T)}");
        }

    }
}
