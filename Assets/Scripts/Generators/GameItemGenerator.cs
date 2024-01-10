
using System.Collections.Generic;
using Ventura.GameLogic.Entities;

namespace Ventura.Generators
{
    public class GameItemGenerator
    {
        private static GameItemGenerator _instance = new GameItemGenerator();
        public static GameItemGenerator Instance { get => _instance; }

        public List<GameItem> GenerateItems(int nItems = 10)
        {
            var itemTemplate = GameItemTemplate.Load("healingPotion");

            var res = new List<GameItem>();

            for (var i = 0; i < nItems; i++)
            {
                res.Add(new GameItem(itemTemplate));
            }

            return res;
        }


    }

}
