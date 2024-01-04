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
            EventManager.GameStateUpdateEvent.AddListener(onGameStateUpdate);
        }

        private void OnDisable()
        {
            EventManager.GameStateUpdateEvent.RemoveListener(onGameStateUpdate);
        }


        private void onGameStateUpdate(GameStateUpdateData updateData)
        {
            if (!(updateData is SkillsUpdateData))
                return;

            var skillsData = ((SkillsUpdateData)updateData).Skills;
            if (!(skillsData.Parent is Player))
                return;

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
