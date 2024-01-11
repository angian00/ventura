using UnityEngine;
using Ventura.GameLogic.Entities;
using Ventura.Unity.Events;

namespace Ventura.Unity.Behaviours
{

    public class PlayerStatsPanelBehaviour : MonoBehaviour
    {
        public Transform hpBar;

        private void OnEnable()
        {
            EventManager.Subscribe<EntityUpdate>(onEntityUpdated);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<EntityUpdate>(onEntityUpdated);
        }


        //----------------- EventSystem notification listeners -----------------

        public void onEntityUpdated(EntityUpdate updateData)
        {
            if (updateData.entity is Player)
            {
                var player = (Player)updateData.entity;
                updateHPBar(player.CurrHP, player.MaxHP);
            }
        }

        //--------------------------------------------------------------------

        private void updateHPBar(int currHP, int maxHP)
        {
            var maxHPRT = hpBar.Find("MaxHP") as RectTransform;
            var currHPRT = hpBar.Find("CurrHP") as RectTransform;

            var newWidth = maxHPRT.sizeDelta.x * currHP / maxHP;
            if (newWidth < 0)
                newWidth = 0;

            currHPRT.sizeDelta = new Vector2(newWidth, currHPRT.sizeDelta.y);
        }
    }
}
