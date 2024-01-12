using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ventura.Unity.ScriptableObjects;
using Ventura.Util;

namespace Ventura.Test
{


    public class ThemePreviewerManager : MonoBehaviour
    {
        public enum ScreenId
        {
            Inventory,
            Settings,
        }


        public GameObject previewScreenContainer;
        public TMP_Dropdown themeDropdown;
        public TMP_Dropdown screenDropdown;

        public ThemePreviewerConfig themePreviewerConfig;

        private Dictionary<string, Transform> _previewScreenObjs = new();
        private Dictionary<string, TMP_FontAsset> _fontCache = new();


        private static List<string> _previewScreenNames = new List<string>() { "Inventory", "Settings" };


        void Start()
        {
            findScreenObjs();

            screenDropdown.onValueChanged.AddListener(delegate { onScreenChosen(); });
            populateDropdown(screenDropdown, _previewScreenNames);

            themeDropdown.onValueChanged.AddListener(delegate { onThemeChosen(); });
            populateDropdown(themeDropdown, themePreviewerConfig.GetKeys());

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

            foreach (var screenObj in _previewScreenObjs.Values)
                applyTheme(themePreviewerConfig.Get(themeName), screenObj);

        }


        private void findScreenObjs()
        {
            //FIXME: findScreenObjs
            foreach (var screenName in _previewScreenNames)
            {
                var screenObj = previewScreenContainer.transform.Find($"{screenName} Screen");
                _previewScreenObjs.Add(screenName, screenObj);
            }
        }


        private void applyTheme(ThemeConfig themeConfig, Transform screenObj)
        {
            screenObj.GetComponent<Image>().color = themeConfig.colors.Get("backgroundColor");
            screenObj.Find("Title").GetComponent<TextMeshProUGUI>().color = themeConfig.colors.Get("titleColor");
            screenObj.Find("Subtitle").GetComponent<TextMeshProUGUI>().color = themeConfig.colors.Get("subtitleColor");

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
