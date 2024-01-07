using UnityEngine;
using Ventura.GameLogic.Entities;
using Ventura.Util;

namespace Ventura.GameLogic.Actions
{
    public record ActionResult
    {
        private bool _success;
        private string? _reason;

        public bool Success { get => _success; }
        public string? Reason { get => _reason; }


        public ActionResult(bool success, string? reason = null)
        {
            _success = success;
            _reason = reason;
        }
    }


    public enum GameActionType
    {
        WaitAction,
        BumpAction,
        MovementAction,
        MeleeAction,
        EnterMapAction,
        ExitMapAction,

        DropItemAction,
        UseItemAction,
        EquipItemAction,
        PickupItemAction,
        CombineItemsAction,
    }


    public class ActionData
    {
        protected GameActionType _actionType;
        public GameActionType ActionType { get => _actionType; set => _actionType = value; }

        private Vector2Int? _deltaPos;
        public Vector2Int? DeltaPos { get => _deltaPos; set => _deltaPos = value; }

        private GameItem? _targetItem;
        public GameItem? TargetItem { get => _targetItem; set => _targetItem = value; }


        public ActionData(GameActionType actionType)
        {
            this._actionType = actionType;
        }

        public void CheckActionType(GameActionType targetType)
        {
            if (_actionType != targetType)
                throw new GameException("Inconsistency in Action Type",
                    DataUtils.EnumToStr(targetType),
                    DataUtils.EnumToStr(_actionType));
        }

        public override string ToString()
        {
            string res = DataUtils.EnumToStr(_actionType);
            if (_deltaPos != null)
                res += $" DeltaPos: {_deltaPos}";
            if (_deltaPos != null)
                res += $" TargetItem: {_targetItem}";

            return res;
        }
    }


    public interface GameAction
    {
        //protected Actor _actor;
        //public Actor Actor { get => _actor; }

        //protected GameAction(Actor actor)
        //{
        //    this._actor = actor;
        //}

        public ActionResult Perform(Actor actor, ActionData actionData, GameState gameState);
    }


    public class WaitAction : GameAction
    {
        public ActionResult Perform(Actor actor, ActionData actionData, GameState _)
        {
            Debug.Assert(actionData.ActionType == GameActionType.WaitAction);
            //do nothing, spend a turn
            return new ActionResult(true, $"{actor.Name} is waiting... ");
        }
    }

}
