using UnityEngine;
using Ventura.Util;

namespace Ventura.GameLogic.Actions
{
    public abstract class DirectionAction : GameAction
    {
        protected int _dx;
        protected int _dy;

        public Vector2Int TargetXY { get => new Vector2Int(_actor.x + _dx, _actor.y + _dy); }
        public Actor? TargetActor  { get => (_orch.CurrMap == null ? null : _orch.CurrMap.GetActorAt(TargetXY)); }
        public Site? TargetSite    { get => (_orch.CurrMap == null ? null : _orch.CurrMap.GetSiteAt(TargetXY)); }

        public DirectionAction(Orchestrator orch, Actor actor, int dx, int dy) : base(orch, actor)
        {
            this._dx = dx;
            this._dy = dy;
        }
    }


    public class BumpAction : DirectionAction
    {
        public BumpAction(Orchestrator orch, Actor actor, int dx, int dy) : base(orch, actor, dx, dy) { }

        public override ActionResult Perform()
        {
            if (TargetActor != null)
                return (new MeleeAction(_orch, _actor, _dx, _dy)).Perform();

            else if (TargetSite != null)
                return (new EnterMapAction(_orch, _actor, _dx, _dy)).Perform();

            else
                return (new MovementAction(_orch, _actor, _dx, _dy)).Perform();
        }
    }

    public class MeleeAction : DirectionAction
    {
        public MeleeAction(Orchestrator orch, Actor actor, int dx, int dy) : base(orch, actor, dx, dy) { }

        public override ActionResult Perform()
        {
            Messages.Log("TODO: MeleeAction.Perform()");
            return new ActionResult(false, "TODO: implement MeleeAction");
        }
    }

    public class MovementAction : DirectionAction
    {
        public MovementAction(Orchestrator orch, Actor actor, int dx, int dy) : base(orch, actor, dx, dy) { }

        public override ActionResult Perform()
        {
            if (_orch.CurrMap == null)
                return new ActionResult(false, "Invalid map");

            if (!_orch.CurrMap.IsInBounds(TargetXY.x, TargetXY.y))
                return (new ExitMapAction(_orch, _actor, _dx, _dy)).Perform();

            if (!_orch.IsWalkable(TargetXY.x, TargetXY.y))
                return new ActionResult(false, "That way is blocked");

            _orch.MoveActorTo(_actor, TargetXY.x, TargetXY.y);

            return new ActionResult(true);
        }
    }

    public class EnterMapAction : DirectionAction
    {
        public EnterMapAction(Orchestrator orch, Actor actor, int dx, int dy) : base(orch, actor, dx, dy) { }

        public override ActionResult Perform()
        {
            var targetSite = this.TargetSite;
            if (targetSite == null)
                return new ActionResult(false, "No site to enter");

            _orch.MoveActorTo(_actor, TargetXY.x, TargetXY.y);

            Messages.Display($"Entering {targetSite.Name }");
            _orch.EnterMap(targetSite.Name);

            return new ActionResult(true);
        }
    }

    public class ExitMapAction : DirectionAction
    {
        public ExitMapAction(Orchestrator orch, Actor actor, int dx, int dy) : base(orch, actor, dx, dy) { }

        public override ActionResult Perform()
        {
            if (_orch.World.MapStackSize > 1)
            {
                Messages.Display($"Returning to {_orch.World.CurrMap.Name}");
                _orch.ExitMap();

                return new ActionResult(true);
            }
            else
            {
                return new ActionResult(false, "That way is blocked");
            }
        }
       
    }
}
