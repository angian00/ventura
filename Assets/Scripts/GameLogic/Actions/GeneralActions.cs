namespace Ventura.GameLogic.Actions
{
#nullable enable
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

        protected Orchestrator _orch;

        protected GameAction(Orchestrator orch, Actor actor)
        {
            this._orch = orch;
            this._actor = actor;
        }

        public abstract ActionResult Perform();
    }


    public class WaitAction : GameAction
    {
        public WaitAction(Orchestrator orch, Actor actor) : base(orch, actor) { }

        public override ActionResult Perform()
        {
            //do nothing, spend a turn
            return new ActionResult(true, $"{_actor.Name} is waiting... ");
        }
    }
}
