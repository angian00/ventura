﻿using System;
using UnityEngine;
using Ventura.GameLogic.Components;
using Ventura.Util;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public abstract class EntityTemplate
    {
        [SerializeField]
        protected string _name;
        public string Name { get => _name; }

        [SerializeField]
        protected string _spriteId;
        public string SpriteId { get => _spriteId; }

        [SerializeField]
        protected string _baseColor;
        public string BaseColor { get => _baseColor; }

    }

    [Serializable]
    public class MonsterTemplate : EntityTemplate, ISerializationCallbackReceiver
    {

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


    public class GameItemTemplate : EntityTemplate { }

}



