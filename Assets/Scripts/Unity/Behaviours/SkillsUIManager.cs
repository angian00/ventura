
using TMPro;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class SkillsUIManager : MonoBehaviour
    {
        private Orchestrator _orch;
        private TextMeshProUGUI _debugText;

        void Start()
        {
            _orch = Orchestrator.Instance;
            _debugText = transform.Find("Debug Text").GetComponent<TextMeshProUGUI>();

            updateData();
        }

        private void updateData()
        {
            var skills = _orch.Player.Skills;
            if (skills == null)
            {
                DebugUtils.Error("No Skills component found in player");
                return;
            }

            var msg = "";
            foreach (var skillId in skills.SkillIds)
            {
                DebugUtils.Log($"Found in skills: {skillId}");

                msg += $"{skillId}: {skills.GetSkillValue(skillId)} \n";
                msg += "\n";
            }

            _debugText.text = msg;
        }
    }
}
