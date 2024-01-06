using UnityEngine;

namespace Ventura.GameLogic.Actions
{
    public abstract class DirectionAction : GameAction
    {
        public abstract ActionResult Perform(Actor actor, ActionData actionData, GameState gameState);

        protected static Vector2Int GetTargetPos(Actor actor, ActionData actionData)
        {
            Debug.Assert(actionData.DeltaPos != null);
            var deltaPos = (Vector2Int)actionData.DeltaPos;

            return new Vector2Int(actor.x + deltaPos.x, actor.y + deltaPos.y);
        }

        protected static Actor? GetTargetActor(Actor actor, ActionData actionData, GameState gameState)
        {
            var currMap = gameState.CurrMap;
            if (currMap == null)
                return null;

            return currMap.GetAnyEntityAt<Actor>(GetTargetPos(actor, actionData));
        }

        protected static Site? GetTargetSite(Actor actor, ActionData actionData, GameState gameState)
        {
            var currMap = gameState.CurrMap;
            if (currMap == null)
                return null;

            return currMap.GetAnyEntityAt<Site>(GetTargetPos(actor, actionData));
        }
    }


    public class BumpAction : DirectionAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.BumpAction);

            if (GetTargetActor(actor, actionData, gameState) != null)
            {
                actionData.ActionType = GameActionType.MeleeAction;
                return (new MeleeAction()).Perform(actor, actionData, gameState);
            }
            else
            {
                actionData.ActionType = GameActionType.MovementAction;
                return (new MovementAction()).Perform(actor, actionData, gameState);
            }
        }
    }

    public class MeleeAction : DirectionAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.MeleeAction);
            return new ActionResult(false, "TODO: implement MeleeAction");
        }
    }

    public class MovementAction : DirectionAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.MovementAction);

            var currMap = gameState.CurrMap;
            if (currMap == null)
                return new ActionResult(false, "Invalid map");

            var targetPos = GetTargetPos(actor, actionData);

            ////go up to parent map if you try to go out of bounds
            //if (!currMap.IsInBounds(targetPos.x, targetPos.y))
            //{
            //    actionData.ActionType = GameActionType.ExitMapAction;
            //    return (new ExitMapAction()).Perform(actor, actionData, gameState);
            //}

            if (!currMap.IsWalkable(targetPos.x, targetPos.y))
                return new ActionResult(false, "That way is blocked");


            gameState.MoveActorTo(actor, targetPos.x, targetPos.y);

            return new ActionResult(true);
        }
    }

    public class EnterMapAction : DirectionAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.EnterMapAction);

            var targetPos = GetTargetPos(actor, actionData);
            var targetSite = GetTargetSite(actor, actionData, gameState);
            if (targetSite == null)
                return new ActionResult(false, "No site to enter");

            gameState.MoveActorTo(actor, targetPos.x, targetPos.y);
            gameState.EnterMap(targetSite.Name);

            return new ActionResult(true, $"{actor.Name} enters {targetSite.Name}");
        }
    }

    public class ExitMapAction : DirectionAction
    {
        public override ActionResult Perform(Actor actor, ActionData actionData, GameState gameState)
        {
            actionData.CheckActionType(GameActionType.ExitMapAction);

            if (gameState.CurrMapStack.Count > 1)
            {
                gameState.ExitMap();

                return new ActionResult(true, $"{actor.Name} returns to {gameState.CurrMapStack.CurrMapName}");
            }
            else
            {
                return new ActionResult(false, "Already at top level map");
            }
        }

    }
}
