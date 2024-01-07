using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
            EventManager.Subscribe<GameStateUpdate>(onGameStateUpdated);
            EventManager.Subscribe<InfoResponse>(onInfoResponse);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<GameStateUpdate>(onGameStateUpdated);
            EventManager.Unsubscribe<InfoResponse>(onInfoResponse);
        }


        //----------------- EventSystem notification listeners -----------------

        public void onGameStateUpdated(GameStateUpdate updateData)
        {
            if (updateData.updatedFields.HasFlag(GameStateUpdate.UpdatedFields.Location))
            {
                onLocationUpdate(updateData.mapStackNames);
            }
        }

        private void onLocationUpdate(List<string> mapStackNames)
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

        public void onInfoResponse(InfoResponse infoData)
        {
            if (infoData.infoType == InfoType.TileContent)
            {
                var tileInfoData = (TileInfoResponse)infoData;
                tileInfo.text = getTileInfoStr(tileInfoData);
                entityInfo.text = getEntityInfoStr(tileInfoData);
            }
        }



        //--------------------------------------------------------------------

        private string getTileInfoStr(TileInfoResponse tileInfoData)
        {
            if (tileInfoData.pos == null)
                return "";

            var res = $"x: {((Vector2Int)tileInfoData.pos).x}, y: {((Vector2Int)tileInfoData.pos).y}";

            if (tileInfoData.terrain != null)
                res += $" - {tileInfoData.terrain}";

            return res;
        }


        private string getEntityInfoStr(TileInfoResponse tileInfoData)
        {
            if (tileInfoData.pos == null)
                return "";

            if (tileInfoData.actor != null)
                return tileInfoData.actor;

            if (tileInfoData.site != null)
                return tileInfoData.site;

            if (tileInfoData.items != null && tileInfoData.items.Count > 0)
                return $"<{tileInfoData.items[0]}>";

            return "";
        }
    }
}
