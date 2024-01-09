using UnityEngine;
using Ventura.Unity.Events;

namespace Ventura.Unity.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Ventura/Add ScriptableObjects Asset/SeverityColorConfig")]
    public class SeverityColorConfig : GameConfigMap<TextNotification.Severity, Color> { }
}