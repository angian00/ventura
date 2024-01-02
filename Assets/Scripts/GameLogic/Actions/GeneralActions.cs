using UnityEngine;
using Ventura.Unity.Behaviours;

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


    public abstract class GameAction
    {
        protected Actor _actor;
        public Actor Actor { get => _actor; }

        protected GameAction(Actor actor)
        {
            this._actor = actor;
        }

        public abstract ActionResult Perform();
    }


    public class WaitAction : GameAction
    {
        public WaitAction(Actor actor) : base(actor) { }

        public override ActionResult Perform()
        {
            //do nothing, spend a turn
            return new ActionResult(true, $"{_actor.Name} is waiting... ");
        }
    }

    public class LookAction : GameAction
    {
        protected Vector2Int? _tilePos;

        //CHECK if it can merged with DirectionAction
        public LookAction(Actor actor, Vector2Int? tilePos) : base(actor) 
        {
            _tilePos = tilePos;
        }

        public override ActionResult Perform()
        {
            UIManager.Instance.UpdateTileInfo(Orchestrator.Instance.GameState.CurrMap, _tilePos); //FIXME: refactor

            return new ActionResult(true);
        }
    }
}
