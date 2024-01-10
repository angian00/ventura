using System;
using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Monster : Actor, ISerializationCallbackReceiver
    {
        [NonSerialized]
        protected AI _ai = null;

        [SerializeReference]
        protected MonsterTemplate _template;

        public override string Color { get => _template.BaseColor; }
        public override string SpriteId { get => _template.SpriteId; }

        public override CombatStats CombatStats { get => _template.CombatStats; }


        public Monster(MonsterTemplate template) : base(template.Name)
        {
            initFromTemplate(template);
        }


        private void initFromTemplate(MonsterTemplate template)
        {
            this._template = template;

            _maxHP = template.StartHP;
            _currHP = _maxHP;

            _ai = AIFactory.CreateAI(template.AIType);
            if (_ai != null)
                _ai.Parent = this;
        }


        // -------- Custom Serialization -------------------
        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            initFromTemplate(_template);
        }

        // -------------------------------------------------


        public ActionData ChooseAction(GameState gameState)
        {
            return _ai?.ChooseAction(gameState);
        }
    }
}



