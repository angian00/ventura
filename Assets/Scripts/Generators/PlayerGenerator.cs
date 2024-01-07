using Ventura.GameLogic.Entities;
using Ventura.Util;

namespace Ventura.Generators
{

    public class PlayerGenerator
    {

        public static Player GeneratePlayerWithBooks()
        {
            DebugUtils.Log("GeneratePlayer()");
            var player = new Player("AnGian");
            foreach (var book in BookItemGenerator.Instance.GenerateBooks())
                player.Inventory.AddItem(book);

            return player;
        }
    }
}