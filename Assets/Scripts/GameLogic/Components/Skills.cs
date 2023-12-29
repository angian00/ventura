using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


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

        public ReadOnlyCollection<SkillId> SkillIds { get => new ReadOnlyCollection<SkillId>(_skillValues.Keys.ToList()); }

        public int GetSkillValue(SkillId skillId)
        {
            if (_skillValues.ContainsKey(skillId))
                return _skillValues[skillId];

            return 0;
        }

        public void SetSkillValue(SkillId skillId, int newValue)
        {
            _skillValues[skillId] = newValue;
        }

        public void AddToSkillValue(SkillId skillId, int deltaValue)
        {
            _skillValues[skillId] = GetSkillValue(skillId) + deltaValue;
        }
    }
}

