
using System;

namespace Ventura.Unity.ScriptableObjects
{
    [Serializable]
    public record ThemeConfig
    {
        public StringColorConfig colors;
    }

    public class ThemePreviewerConfig : GameConfigMap<string, ThemeConfig> { }

}