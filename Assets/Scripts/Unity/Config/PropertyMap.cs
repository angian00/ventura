
using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Unity.Config
{
    [CreateAssetMenu(menuName = "Ventura/Add Config Asset/TerrainColorConfig")]
    public class TerrainColorConfig: PropertyMap<TerrainDef.TerrainType, Color>;

    [CreateAssetMenu(menuName = "Ventura/Add Config Asset/SeverityColorConfig")]
    public class SeverityColorConfig: PropertyMap<TextNotification.Severity, Color>;

    [CreateAssetMenu(menuName = "Ventura/Add Config Asset/SpriteConfig")]
    public class SpriteConfig: PropertyMap<string, Sprite>;



    [Serializable]
    public record PropertyListItem<TKey, TValue>
    {
        public T enumValue;
        public Color color;
    }

    public class PropertyMap<TKey, TValue> : ScriptableObject
    {

        [SerializeField]
        private List<PropertyListItem> propertyValues;
        
        private Dictionary<TKey, TValue> _data;

        
        public TValue Get(TKey key) => _data.Get(key);
        public void Add(TKey key, TValue value) => _data.Add(key, value);


        void OnEnable()
        {
            DebugUtils.Log($"Reconstructing GamePropertyMap [{this.name}]");

            _data = new();

            foreach (var key in propertyValues)
                _data.Add(key, value);
        }

    }
}