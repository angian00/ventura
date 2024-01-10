using System;
using UnityEngine;
using Ventura.GameLogic.Components;
using Ventura.Util;

namespace Ventura.GameLogic.Entities
{

    [Serializable]
    public class MonsterTemplate : EntityTemplate, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected int _startHP;
        public int StartHP { get => _startHP; }

        [SerializeField]
        protected CombatStats _combatStats;
        public CombatStats CombatStats { get => _combatStats; }

        [NonSerialized]
        protected AIType _aiType;
        public AIType AIType { get => _aiType; }

        /// -------- Custom Serialization -------------------
        [SerializeField]
        private string __auxAIType;


        public void OnBeforeSerialize()
        {
            __auxAIType = _aiType.ToString();
        }

        public void OnAfterDeserialize()
        {
            _aiType = (AIType)Enum.Parse(typeof(AIType), __auxAIType);
        }

        /// ------------------------------------------------------

        public static MonsterTemplate Load(string templateFileName)
        {
            var fullPath = $"Data/MonsterTemplates/{templateFileName}";
            DebugUtils.Log($"Loading monsterTemplate resource from {fullPath}");

            var jsonFileObj = Resources.Load<TextAsset>(fullPath);
            return JsonUtility.FromJson<MonsterTemplate>(jsonFileObj.text);
        }
    }

}

