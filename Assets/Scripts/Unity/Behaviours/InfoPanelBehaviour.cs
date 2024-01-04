
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class InfoPanelBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI tileInfo;
        public TextMeshProUGUI entityInfo;
        public TextMeshProUGUI locationInfo;


        private void OnEnable()
        {
            EventManager.TileInfoUpdateEvent.AddListener(onMapInfoUpdated);
            EventManager.LocationChangeEvent.AddListener(onLocationChanged);
        }

        private void OnDisable()
        {
            EventManager.TileInfoUpdateEvent.RemoveListener(onMapInfoUpdated);
            EventManager.LocationChangeEvent.RemoveListener(onLocationChanged);
        }


        public void onMapInfoUpdated(string tileInfoStr, string entityInfoStr)
        {
            tileInfo.text = tileInfoStr;
            entityInfo.text = entityInfoStr;
        }


        private void onLocationChanged(GameMap mapData, ReadOnlyCollection<string> mapStackNames)
        {
            string locationInfoStr = "";

            for (int i = 0; i < mapStackNames.Count; i++)
            {
                locationInfoStr += mapStackNames[i];

                if (i < mapStackNames.Count - 1)
                    locationInfoStr += " > ";
            }

            locationInfo.text = locationInfoStr;
        }
    }
}
