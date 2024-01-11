using UnityEngine;
using Ventura.GameLogic.Actions;
using Ventura.GameLogic.Entities;
using Ventura.Unity.Events;
using Debug = UnityEngine.Debug;

namespace Ventura.GameLogic.Components
{
    public class Combat
    {
        public static ActionResult PerformMeleeAttack(Actor attacker, Actor defender, GameState gameState)
        {
            Debug.Assert(defender.CurrHP > 0);

            var rollResult = Random.Range(-5, 6); // uniformly extracted from -5 to +5, both extremes included, to average 0
            var attackResult = rollResult + attacker.CombatStats.Attack - defender.CombatStats.Defense;
            if (attackResult <= 0)
                return new ActionResult(true, $"{attacker.Name} attacks {defender.Name} and misses!", true);

            var damage = Random.Range(1, attacker.CombatStats.Damage + 1); //uniformly extracted from 1 to [Damage] included
            defender.CurrHP -= damage;
            if (defender.CurrHP > 0)
            {
                EventManager.Publish(new EntityUpdate(EntityUpdate.Type.Changed, defender));
                return new ActionResult(true, $"{attacker.Name} attacks {defender.Name} and makes {damage} damage!", true);
            }


            gameState.CurrMap.RemoveEntity(defender);

            //TODO: if (defender is Player) --> game over event
            //FUTURE: drop body

            return new ActionResult(true, $"{attacker.Name} attacks {defender.Name} and makes {damage} damage, killing him!", true);
        }
    }
}

