
using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.Util;

namespace Ventura.Unity.ScriptableObjects
{
    [Serializable]
    public record GameConfigItem<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }

    public class GameConfigMap<TKey, TValue> : ScriptableObject
    {

        [SerializeField]
        private List<GameConfigItem<TKey, TValue>> propertyValues;

        private Dictionary<TKey, TValue> _data;


        public TValue Get(TKey key) => _data[key];
        public void Add(TKey key, TValue value) => _data.Add(key, value);
        public List<TKey> GetKeys() => new List<TKey>(_data.Keys);


        void OnEnable()
        {
            DebugUtils.Log($"Reconstructing GamePropertyMap [{this.name}]");

            _data = new();

            foreach (var property in propertyValues)
                _data.Add(property.key, property.value);
        }

    }
}