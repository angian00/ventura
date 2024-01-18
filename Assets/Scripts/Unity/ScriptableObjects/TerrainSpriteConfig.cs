using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Unity.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ventura/Add ScriptableObjects Asset/TerrainSpriteConfig")]
    public class TerrainSpriteConfig : GameConfigMap<TerrainDef.TerrainType, Sprite> { }

}
