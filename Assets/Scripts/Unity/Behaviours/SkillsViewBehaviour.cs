using TMPro;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Components;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class SkillsViewBehaviour : MonoBehaviour
    {
        public TextMeshProUGUI _debugText;

        private void OnEnable()
        {
            EventManager.Subscribe<EntityUpdate>(onPlayerUpdated);

        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<EntityUpdate>(onPlayerUpdated);
        }


        private void onPlayerUpdated(EntityUpdate updateData)
        {
            if (!(updateData.entity is Player))
                return;

            var skillsData = ((Player)updateData.entity).Skills;
            updateView(skillsData);
        }


        private void updateView(Skills skillsData)
        {
            var msg = "";
            foreach (var skillId in skillsData.SkillIds)
            {
                DebugUtils.Log($"Found in skills: {skillId}");

                msg += $"{skillId}: {skillsData.GetSkillValue(skillId)} \n";
                msg += "\n";
            }

            _debugText.text = msg;
        }
    }
}
