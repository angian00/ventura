using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Entities;
using Ventura.Util;

namespace Ventura.GameLogic.Components
{
    public enum AIType
    {
        Null,
        Static,
        RandomMovement,
        Hostile,
    }


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

        public abstract ActionData? ChooseAction(GameState gameState);
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
                case AIType.Hostile:
                    return new HostileAI();

                default:
                    throw new GameException($"Unknown AIType: {DataUtils.EnumToStr(aiType)}");
            }
        }
    }


    public class StaticAI : AI
    {
        public override ActionData? ChooseAction(GameState gameState)
        {
            return new ActionData(GameActionType.WaitAction);
        }

    }

    public class RandomMovementAI : AI
    {
        public override ActionData? ChooseAction(GameState gameState)
        {
            var actionData = new ActionData(GameActionType.BumpAction);
            actionData.DeltaPos = RandomUtils.RandomMovement();
            //FIXME: retry if direction is not walkable

            return actionData;
        }

    }


    public class HostileAI : AI
    {
        public override ActionData? ChooseAction(GameState gameState)
        {
            var actionData = new ActionData(GameActionType.BumpAction);
            actionData.DeltaPos = RandomUtils.RandomMovement();

            return actionData;
        }

    }
}
