using System.Collections.Generic;
using Ventura.GameLogic.Actions;


namespace Ventura.GameLogic.Components
{
    public class Skills
    {
        protected Actor _parent;
        protected Dictionary<SkillId, int> _skillValues = new();


        public Skills(Actor parent)
        {
            this._parent = parent;
        }

        public int GetSkill(SkillId skillId)
        {
            if (_skillValues.ContainsKey(skillId))
                return _skillValues[skillId];

            return 0;
        }

        public void SetSkill(SkillId skillId, int newValue)
        {
            _skillValues[skillId] = newValue;
        }

        public void AddToSkill(SkillId skillId, int deltaValue)
        {
            _skillValues[skillId] = GetSkill(skillId) + deltaValue;
        }
    }
}

