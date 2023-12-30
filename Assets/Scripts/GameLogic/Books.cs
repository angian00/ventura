

using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.Util;
using static UnityEngine.Rendering.DebugUI;

namespace Ventura.GameLogic
{
    public enum SkillId
    {
        Latin,
        Greek,
    }


    public class BookItem : GameItem
    {
        public BookItem(string name, SkillId skill, int amount) : base(name)
        {
            _consumable = new BookConsumable(this, skill, amount);
        }
    }


    public class BookConsumable : Consumable
    {
        private SkillId _skill;
        private int _amount;

        public BookConsumable(GameItem parent, SkillId skill, int amount) : base(parent)
        {
            this._skill = skill;
            this._amount = amount;
        }

        public override ActionResult Use(ItemAction action)
        {
            var consumer = action.Actor;

            if (consumer.Skills == null)
                return new ActionResult(false, $"{consumer.Name} cannot use [{_parent.Name}]");

            consumer.Skills.AddToSkillValue(_skill, _amount);

            Consume();
            return new ActionResult(true, $"You read [{_parent.Name}], and gain {_amount} points in the skill [{DataUtils.EnumToStr<SkillId>(_skill)}]");
        }
    }
}
