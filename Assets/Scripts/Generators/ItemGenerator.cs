
using Ventura.GameLogic;
using Random = UnityEngine.Random;

namespace Ventura.Generators
{
    public class ItemGenerator
    {
        public static void GenerateSomeItems(Actor actor)
        {
            var nItems = 10;

            for (var i = 0; i < nItems; i++)
            {
                var newItem = GenerateBookItem();
                actor.Inventory.AddItem(newItem);
            }
        }


        private static BookItem GenerateBookItem()
        {
            return new BookItem(DummyNameGenerator.GenerateName(), SkillId.Latin, Random.Range(1, 50));
        }
    }

}