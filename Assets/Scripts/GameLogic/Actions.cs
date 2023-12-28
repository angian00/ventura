using UnityEngine;
using Ventura.Util;
using Ventura.Generators;

namespace Ventura.GameLogic
{
#nullable enable
    public record ActionResult
    {
        private bool _success;
        private string? _reason;

        public bool Success { get { return _success; } }
        public string? Reason { get { return _reason; } }


        public ActionResult(bool success, string? reason=null)
        {
            _success = success;
            _reason = reason;
        }
    }


    public abstract class Action
    {
        protected Actor _actor;
        protected Orchestrator _orch;

        protected Action(Orchestrator orch, Actor actor)
        {
            this._orch = orch;
            this._actor = actor;
        }

        public abstract ActionResult Perform();
    }


    public class WaitAction: Action
    {
        public WaitAction(Orchestrator orch, Actor actor) : base(orch, actor) { }

        public override ActionResult Perform()
        {
            //do nothing, spend a turn
            if (_orch.CurrMap != null && _orch.CurrMap.Visible[_actor.X, _actor.Y])
                Messages.Display(_actor.Name + " is waiting... ");
    

            return new ActionResult(true);
        }
    }

    //-----------------------------------------------------
    // Direction Actions
    //-----------------------------------------------------

    public abstract class DirectionAction : Action
    {
        protected int _dx;
        protected int _dy;

        public Vector2Int TargetXY { get { return new Vector2Int(_actor.X + _dx, _actor.Y + _dy); } }
        public Actor? TargetActor { get { return _orch.CurrMap == null ? null : _orch.CurrMap.GetActorAt(TargetXY) ; } }
        public Site? TargetSite { get { return _orch.CurrMap == null ? null : _orch.CurrMap.GetSiteAt(TargetXY) ; } }

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
