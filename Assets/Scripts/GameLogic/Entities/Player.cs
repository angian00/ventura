using System;
using Ventura.GameLogic.Components;

namespace Ventura.GameLogic.Entities
{
    [Serializable]
    public class Player : Actor
    {
        private static string PlayerColorStr = "#ffffff"; //TODO: move to configuration
        private static CombatStats PlayerCombatStats = new CombatStats(5, 4, 3);
        private static int PlayerMaxHP = 10;


        public override string Color { get => PlayerColorStr; }
        public override string SpriteId { get => "player"; }

        public override CombatStats CombatStats { get => PlayerCombatStats; }

        public Player(string name) : base(name)
        {
            _maxHP = PlayerMaxHP;
            _currHP = PlayerMaxHP;

            _inventory = new Inventory(this, 999);
            _skills = new Skills(this);
        }

    }
}
