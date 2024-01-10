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


        public Monster(MonsterTemplate template) : base(template.Name)
        {
            this._template = template;
            initFromTemplate();
        }


        private void initFromTemplate()
        {
            _ai = AIFactory.CreateAI(_template.AIType);
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

            initFromTemplate();
        }

        // -------------------------------------------------


        public ActionData ChooseAction()
        {
            return _ai?.ChooseAction();
        }
    }
}



