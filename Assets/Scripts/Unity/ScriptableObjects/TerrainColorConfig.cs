using UnityEngine;
using Ventura.GameLogic;

namespace Ventura.Unity.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ventura/Add ScriptableObjects Asset/TerrainColorConfig")]
    public class TerrainColorConfig : GameConfigMap<TerrainDef.TerrainType, Color> { }
}