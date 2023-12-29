
using TMPro;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Behaviours
{

    public class SkillsUIManager : MonoBehaviour, ModalUIManager
    {
        private Orchestrator _orch;
        private TextMeshProUGUI _debugText;

        void Start()
        {
            _orch = Orchestrator.GetInstance();
            _debugText = transform.Find("Debug Text").GetComponent<TextMeshProUGUI>();

            UpdateData();
        }

        public void UpdateData()
        {
            var skills = _orch.Player.Skills;
            if (skills == null)
            {
                DebugUtils.Error("No skills found in player");
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
