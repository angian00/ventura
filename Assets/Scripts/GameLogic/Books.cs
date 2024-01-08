﻿

using System;
using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;
using Ventura.GameLogic.Entities;
using Ventura.Util;

namespace Ventura.GameLogic
{
    public enum SkillId
    {
        Latin,
        Greek,
    }

    [Serializable]
    public class BookItem : GameItem
    {
        [SerializeField]
        protected string _title;

        [SerializeField]
        protected string _author;

        public override string Label
        {
            get
            {
                var res = "book";

                if (_author != null)
                    res += $" by {_author}";
                res += $" - '{_title}'";

                return res;
            }
        }


        public BookItem(string title, string? author, SkillId skill, int amount) : base("book")
        {
            this._title = title;
            this._author = author;

            _consumable = new BookConsumable(this, skill, amount);
        }
    }


    [Serializable]
    public class BookConsumable : Consumable
    {
        [SerializeField]
        private SkillId _skill;

        [SerializeField]
        private int _amount;

        public BookConsumable(GameItem parent, SkillId skill, int amount) : base(parent)
        {
            this._skill = skill;
            this._amount = amount;
        }

        public override ActionResult Use(Actor consumer, ItemAction action)
        {
            if (consumer.Skills == null)
                return new ActionResult(false, $"{consumer.Name} cannot use [{_parent.Label}]");

            consumer.Skills.AddToSkillValue(_skill, _amount);

            Consume();

            return new ActionResult(true, $"You read [{_parent.Label}], and gain {_amount} points in the skill [{DataUtils.EnumToStr<SkillId>(_skill)}]");
        }
    }
}
