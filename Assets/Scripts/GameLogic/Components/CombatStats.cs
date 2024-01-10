using System;
using UnityEngine;

namespace Ventura.GameLogic.Components
{
    [Serializable]
    public class CombatStats
    {
        [SerializeField]
        protected int _attack;
        public int Attack { get => _attack; }

        [SerializeField]
        protected int _defense;
        public int Defense { get => _defense; }

        [SerializeField]
        protected int _damage;
        public int Damage { get => _damage; }


        public CombatStats(int attack, int defense, int damage) //FUTURE: remove when CombatStats will be always unserialized from file
        {
            _attack = attack;
            _defense = defense;
            _damage = damage;
        }
    }
}

