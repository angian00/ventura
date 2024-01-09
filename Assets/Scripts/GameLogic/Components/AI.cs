using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Entities;
using Ventura.Util;

namespace Ventura.GameLogic.Components
{
    public abstract class AI
    {
        protected Actor? _parent;
        public Actor? Parent { get => _parent; set => _parent = value; }


        protected AI()
        {
        }

        protected AI(Actor parent)
        {
            this._parent = parent;
        }

        public abstract ActionData? ChooseAction();
    }

    public enum AIType
    {
        Null,
        Static,
        RandomMovement,
    }


    public class AIFactory
    {
        public static AI? CreateAI(AIType aiType)
        {
            switch (aiType)
            {
                case AIType.Null:
                    return null;
                case AIType.Static:
                    return new StaticAI();
                case AIType.RandomMovement:
                    return new RandomMovementAI();

                default:
                    throw new GameException($"Unknown AIType: {DataUtils.EnumToStr(aiType)}");
            }
        }
    }
    //public class EnemyAI : AI
    //{
    //    private List<Vector2Int> _path = new();

    //    public EnemyAI(Actor parent) : base(parent) { }


    //    public override GameAction? ChooseAction()
    //    {
    //        DebugUtils.Log("EnemyAI.chooseAction");

    //        //var gameState = Orchestrator.Instance.GameState; //FIXME

    //        var target = gameState.Player;
    //        var gameMap = gameState.CurrMap;

    //        var dx = target.x - _parent.y;
    //        var dy = target.y - _parent.y;

    //        var distance = Math.Max(Math.Abs(dx), Math.Abs(dy));


    //        if (gameMap.Visible[_parent.x, _parent.y])
    //        {
    //            //if monster is visible to player, 
    //            //then player is visible to monster
    //            if (distance <= 1)
    //                return new MeleeAction(_parent, dx, dy);


    //            _path = GetPathTo(gameMap, target.x, target.y);
    //        }

    //        if (_path.Count > 0)
    //        {
    //            var dest = _path[0];
    //            _path.RemoveAt(0);

    //            DebugUtils.Log("EnemyAI chose MovementAction");
    //            return new MovementAction(_parent, dest.x - _parent.x, dest.y - _parent.y);
    //        }

    //        return new WaitAction(_parent);
    //    }
    //}


    public class StaticAI : AI
    {
        //public StaticAI(Actor parent) : base(parent) { }

        public override ActionData? ChooseAction()
        {
            return new ActionData(GameActionType.WaitAction);
        }

    }

    public class RandomMovementAI : AI
    {
        //public RandomMovementAI(Actor parent) : base(parent) { }

        public override ActionData? ChooseAction()
        {
            var actionData = new ActionData(GameActionType.BumpAction);
            actionData.DeltaPos = DataUtils.RandomMovement();

            return actionData;
        }

    }
}
