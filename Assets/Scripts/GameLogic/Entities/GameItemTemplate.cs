using System;
using UnityEngine;
using Ventura.Util;

namespace Ventura.GameLogic.Entities
{

    [Serializable]
    public class GameItemTemplate : EntityTemplate
    {
        public static GameItemTemplate Load(string templateFileName)
        {
            var fullPath = $"Data/GameItemTemplates/{templateFileName}";
            DebugUtils.Log($"Loading gameItemTemplate resource from {fullPath}");

            var jsonFileObj = Resources.Load<TextAsset>(fullPath);
            return JsonUtility.FromJson<GameItemTemplate>(jsonFileObj.text);
        }
    }

}
