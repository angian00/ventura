using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Ventura.Util;

namespace Ventura.GameLogic.Components
{
    [Serializable]
    public class Skills: ISerializationCallbackReceiver
    {
        protected Actor _parent;
        public Actor Parent { get => _parent; set => _parent = value; }

        protected Dictionary<SkillId, int> _skillValues = new();


        public Skills(Actor parent)
        {
            this._parent = parent;
        }

        // -------- Custom Serialization -------------------
        
        [SerializeField]
        private List<SkillId> __auxSkillKeys;
        [SerializeField]
        private List<int> __auxSkillValues;


        public void OnBeforeSerialize()
        {
            __auxSkillKeys = new List<SkillId>();
            __auxSkillValues = new List<int>();

            foreach (var skillId in _skillValues.Keys)
            {
                __auxSkillKeys.Add(skillId);
                __auxSkillValues.Add(_skillValues[skillId]);
            }
        }

        public void OnAfterDeserialize()
        {
            Debug.Assert(__auxSkillKeys.Count == __auxSkillValues.Count);

            _skillValues = new();
            for (var i=0; i < __auxSkillKeys.Count; i++)
                _skillValues.Add(__auxSkillKeys[i], __auxSkillValues[i]);
        }

        // -------------------------------------------------

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


        public void Dump()
        {
            foreach (var skillId in _skillValues.Keys)
                DebugUtils.Log($"{skillId}: {_skillValues[skillId]}");
        }
    }
}

