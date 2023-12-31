﻿

using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.Util;

namespace Ventura.GameLogic
{
    public enum SkillId
    {
        Latin,
        Greek,
    }


    public class BookItem : GameItem
    {
        protected string _title;
        protected string _author;

        public override string Label { get => _author == null ? _title : $"{_author} - '{_title}'"; }

        public BookItem(string name, string title, string? author, SkillId skill, int amount) : base(name)
        {
            this._title = title;
            this._author = author;

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
