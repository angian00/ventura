
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
            var res = new List<Monster>();

            for (var i = 0; i < nMonsters; i++)
            {
                res.Add(GenerateMonster());
            }

            return res;
        }


        private Monster GenerateMonster()
        {
            var m = new Monster("butterfly");

            return m;
        }
    }

}
