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
            EventManager.GameStateUpdateEvent.AddListener(onGameStateUpdated);
        }

        private void OnDisable()
        {
            EventManager.GameStateUpdateEvent.AddListener(onGameStateUpdated);
        }


        //----------------- EventSystem notification listeners -----------------

        public void onGameStateUpdated(GameStateUpdateData updateData)
        {
            if (updateData is TileUpdateData)
            {
                onTileUpdate((TileUpdateData)updateData);
            }
            else if (updateData is LocationUpdateData)
            {
                onLocationUpdate((LocationUpdateData)updateData);
            }
        }

        public void onTileUpdate(TileUpdateData updateData)
        {
            tileInfo.text = getTileInfoStr(updateData); ;
            entityInfo.text = getEntityInfoStr(updateData);
        }


        private void onLocationUpdate(LocationUpdateData updateData)
        {
            var mapStackNames = updateData.MapStackNames;

            string locationInfoStr = "";

            for (int i = 0; i < mapStackNames.Count; i++)
            {
                locationInfoStr += mapStackNames[i];

                if (i < mapStackNames.Count - 1)
                    locationInfoStr += " > ";
            }

            locationInfo.text = locationInfoStr;
        }


        //--------------------------------------------------------------------

        private string getTileInfoStr(TileUpdateData updateData)
        {
            var res = $"x: {updateData.Pos.x}, y: {updateData.Pos.y}";

            if (updateData.Terrain != null)
                res += $" - {updateData.Terrain}";

            return res;
        }


        private string getEntityInfoStr(TileUpdateData updateData)
        {
            if (updateData.Actor != null)
                return updateData.Actor;

            if (updateData.Site != null)
                return updateData.Site;

            if (updateData.Items != null && updateData.Items.Count > 0)
                return $"<{updateData.Items[0]}>";

            return "";
        }
    }
}
