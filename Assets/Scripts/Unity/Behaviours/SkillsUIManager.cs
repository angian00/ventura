
using System;
using TMPro;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{

    public class SkillsUIManager : MonoBehaviour, SecondaryUIManager
    {
        [NonSerialized]
        private Player _playerData;
        public Player PlayerData { set => _playerData = value; }

        public TextMeshProUGUI _debugText;


        void Start()
        {
            updateView();
        }


        void Update()
        {
            //if (Orchestrator.Instance.PendingUpdates.Contains(PendingUpdateId.Inventory))
            //    updateView();
        }


        private void updateView()
        {
            var skillsData = _playerData.Skills;
            Debug.Assert(skillsData != null);

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
