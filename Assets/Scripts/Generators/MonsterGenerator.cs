
using System.Collections.Generic;
using Ventura.GameLogic.Entities;

namespace Ventura.Generators
{
    public class MonsterGenerator
    {
        private static MonsterGenerator _instance = new MonsterGenerator();
        public static MonsterGenerator Instance { get => _instance; }

        public List<Monster> GenerateMonsters(int nMonsters = 10)
        {
            //var monsterTemplate = MonsterTemplate.Load("butterfly");
            var monsterTemplate = MonsterTemplate.Load("goblin");

            var res = new List<Monster>();

            for (var i = 0; i < nMonsters; i++)
            {
                res.Add(new Monster(monsterTemplate));
            }

            return res;
        }


    }

}
