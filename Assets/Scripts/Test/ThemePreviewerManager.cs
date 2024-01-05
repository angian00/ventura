using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Unity.Graphics;
using Ventura.Util;

namespace Ventura.Test
{

    public class ThemePreviewerManager : MonoBehaviour
    {
        public TextAsset configFile;

        public GameObject previewScreenContainer;
        public TMP_Dropdown themeDropdown;
        public TMP_Dropdown screenDropdown;

        private ThemePreviewerConfig _configData;
        private Dictionary<string, Transform> _previewScreenObjs = new();
        private Dictionary<string, TMP_FontAsset> _fontCache = new();


        [Serializable]
        private record ThemePreviewerConfig
        {
            public List<string> previewScreenNames;
            public List<ThemeConfig> themes;
        }

        [Serializable]
        private record ThemeConfig
        {
            public string name;
            public List<string> colors;
            public string fontName;
            public int fontSize;
        }



        private void Awake()
        {
            loadConfig();
        }


        void Start()
        {
            findScreenObjs();

            screenDropdown.onValueChanged.AddListener(delegate { onScreenChosen(); });
            populateDropdown(screenDropdown, _configData.previewScreenNames);

            var themeNames = new List<string>();
            foreach (var theme in _configData.themes)
                themeNames.Add(theme.name);

            themeDropdown.onValueChanged.AddListener(delegate { onThemeChosen(); });
            populateDropdown(themeDropdown, themeNames);

        }


        private void loadConfig()
        {
            _configData = JsonUtility.FromJson<ThemePreviewerConfig>(configFile.text);
        }


        private void populateDropdown(TMP_Dropdown dropdown, List<string> options)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.RefreshShownValue();

            dropdown.value = -1;
        }


        private void onScreenChosen()
        {
            var screenName = screenDropdown.options[screenDropdown.value].text;
            DebugUtils.Log($"onScreenChosen: {screenName}");

            var currScreenObj = _previewScreenObjs[screenName];
            currScreenObj.SetAsLastSibling();
        }


        private void onThemeChosen()
        {
            var themeName = themeDropdown.options[themeDropdown.value].text;
            DebugUtils.Log($"onThemeChosen: {themeName}");

            //apply theme
            var themeConfig = findThemeConfig(themeName);
            foreach (var screenObj in _previewScreenObjs.Values)
                applyTheme(themeConfig, screenObj);

        }


        private void findScreenObjs()
        {
            foreach (var screenName in _configData.previewScreenNames)
            {
                var screenObj = previewScreenContainer.transform.Find($"{screenName} Screen");
                _previewScreenObjs.Add(screenName, screenObj);
            }
        }

        private ThemeConfig findThemeConfig(string themeName)
        {
            foreach (var themeConfig in _configData.themes)
                if (themeConfig.name == themeName)
                    return themeConfig;

            return null;
        }

        private void applyTheme(ThemeConfig themeConfig, Transform screenObj)
        {
            screenObj.GetComponent<Image>().color = GraphicsConfig.FromHex(themeConfig.colors[0]);
            screenObj.Find("Title").GetComponent<TextMeshProUGUI>().color = GraphicsConfig.FromHex(themeConfig.colors[1]);
            screenObj.Find("Subtitle").GetComponent<TextMeshProUGUI>().color = GraphicsConfig.FromHex(themeConfig.colors[2]);

            var textObjs = screenObj.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var textObj in textObjs)
            {
                //textObj.font = getFont(themeConfig.fontName);
            }
        }

        private TMP_FontAsset getFont(string fontName)
        {
            if (_fontCache.ContainsKey(fontName))
                return _fontCache[fontName];

            var fontResource = Resources.Load<Font>(fontName);
            var fontAsset = TMP_FontAsset.CreateFontAsset(fontResource);
            _fontCache[fontName] = fontAsset;

            return fontAsset;
        }
    }
}
