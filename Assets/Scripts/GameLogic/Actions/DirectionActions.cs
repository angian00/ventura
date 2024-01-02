using UnityEngine;

namespace Ventura.GameLogic.Actions
{
    public abstract class DirectionAction : GameAction
    {
        protected int _dx;
        protected int _dy;

        public Vector2Int TargetXY { get => new Vector2Int(_actor.x + _dx, _actor.y + _dy); }
        public Actor? TargetActor
        {
            get
            {
                var currMap = Orchestrator.Instance.GameState.CurrMap;
                if (currMap == null)
                    return null;

                return currMap.GetAnyEntityAt<Actor>(TargetXY);
            }
        }

        public Site? TargetSite
        {
            get
            {
                var currMap = Orchestrator.Instance.GameState.CurrMap;
                if (currMap == null)
                    return null;

                return currMap.GetAnyEntityAt<Site>(TargetXY);
            }
        }

        public DirectionAction(Actor actor, int dx, int dy) : base(actor)
        {
            this._dx = dx;
            this._dy = dy;
        }
    }


    public class BumpAction : DirectionAction
    {
        public BumpAction(Actor actor, int dx, int dy) : base(actor, dx, dy) { }

        public override ActionResult Perform()
        {
            if (TargetActor != null)
                return (new MeleeAction(_actor, _dx, _dy)).Perform();

            else if (TargetSite != null)
                return (new EnterMapAction(_actor, _dx, _dy)).Perform();

            else
                return (new MovementAction(_actor, _dx, _dy)).Perform();
        }
    }

    public class MeleeAction : DirectionAction
    {
        public MeleeAction(Actor actor, int dx, int dy) : base(actor, dx, dy) { }

        public override ActionResult Perform()
        {
            return new ActionResult(false, "TODO: implement MeleeAction");
        }
    }

    public class MovementAction : DirectionAction
    {
        public MovementAction(Actor actor, int dx, int dy) : base(actor, dx, dy) { }

        public override ActionResult Perform()
        {
            var gameState = Orchestrator.Instance.GameState;
            var currMap = gameState.CurrMap;

            if (currMap == null)
                return new ActionResult(false, "Invalid map");

            if (!currMap.IsInBounds(TargetXY.x, TargetXY.y))
                return (new ExitMapAction(_actor, _dx, _dy)).Perform();

            if (!currMap.IsWalkable(TargetXY.x, TargetXY.y))
                return new ActionResult(false, "That way is blocked");

            gameState.MoveActorTo(_actor, TargetXY.x, TargetXY.y);

            return new ActionResult(true);
        }
    }

    public class EnterMapAction : DirectionAction
    {
        public EnterMapAction(Actor actor, int dx, int dy) : base(actor, dx, dy) { }

        public override ActionResult Perform()
        {
            var gameState = Orchestrator.Instance.GameState;

            var targetSite = this.TargetSite;
            if (targetSite == null)
                return new ActionResult(false, "No site to enter");

            gameState.MoveActorTo(_actor, TargetXY.x, TargetXY.y);
            gameState.EnterMap(targetSite.Name);

            return new ActionResult(true, $"Entering {targetSite.Name}"); //FUTURE: improve message in case _actor != player
        }
    }

    public class ExitMapAction : DirectionAction
    {
        public ExitMapAction(Actor actor, int dx, int dy) : base(actor, dx, dy) { }

        public override ActionResult Perform()
        {
            var gameState = Orchestrator.Instance.GameState;
            if (gameState.CurrMapStack.Count > 1)
            {
                gameState.ExitMap();

                return new ActionResult(true, $"Returning to {gameState.CurrMapStack.CurrMapName}"); //FUTURE: improve message in case _actor != player
            }
            else
            {
                return new ActionResult(false, "That way is blocked");
            }
        }

    }
}
