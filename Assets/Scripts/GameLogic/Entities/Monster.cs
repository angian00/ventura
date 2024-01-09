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


        public Monster(MonsterTemplate template) : base(template.Name)
        {
            if (template.BaseColor != null)
            {
                Color c;
                if (ColorUtility.TryParseHtmlString(template.BaseColor, out c))
                    this._color = c;
            }

            this._spriteId = template.SpriteId ?? this._name;

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

            _ai = new RandomMovementAI(); //FIXME: get aiType from template again
            _ai.Parent = this;
        }

        // -------------------------------------------------


        public ActionData ChooseAction()
        {
            return _ai?.ChooseAction();
        }
    }
}



