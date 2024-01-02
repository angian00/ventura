
using System.Collections.Generic;
using Ventura.GameLogic.Actions;
using Ventura.Util;


namespace Ventura.GameLogic
{
    public class Orchestrator
    {
        private static Orchestrator _instance = new Orchestrator();
        public static Orchestrator Instance { get => _instance; }

        private CircularList<Actor> _scheduler = new();
        private Queue<GameAction> _playerActionQueue = new();

        private GameState _gameState;
        public GameState GameState { get => _gameState; set => _gameState = value; }

        private PendingUpdates _pendingUpdates = new PendingUpdates();
        public PendingUpdates PendingUpdates { get => _pendingUpdates; }


        public void Suspend()
        {
            _scheduler.Clear();
            _playerActionQueue.Clear();
        }

        public void Resume()
        {
            ActivateActors(_gameState.CurrMap.GetAllEntities<Actor>());
        }


        public void EnqueuePlayerAction(GameAction a)
        {
            DebugUtils.Log("EnqueuePlayerAction");
            _playerActionQueue.Enqueue(a);
        }


        public GameAction? DequeuePlayerAction()
        {
            if (_playerActionQueue.Count > 0)
                return _playerActionQueue.Dequeue();

            return null;
        }


        public void ActivateActor(Actor actor)
        {
            _scheduler.Add(actor);
        }

        public void ActivateActors(IEnumerable<Actor> actors)
        {
            foreach (var a in actors)
                _scheduler.Add(a);
        }

        public void DeactivateAllActors()
        {
            _scheduler.Clear();
        }

        public void ProcessTurn()
        {
            var currActor = _scheduler.Next();
            if (currActor != null)
                currActor.Act();
        }
    }
}
