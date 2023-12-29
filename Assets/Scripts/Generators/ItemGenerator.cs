using System.IO;
using Random = UnityEngine.Random;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Ventura.Util;
using Ventura.GameLogic;

namespace Ventura.Generators
{
    public class ItemGenerator
    {
        public static void GenerateSomeItems(Actor actor)
        {
            var newItem = GenerateBookItem();
            actor.Inventory.AddItem(newItem);
        }


        private static BookItem GenerateBookItem()
        {
            return new BookItem("Test Skill Book", SkillId.Latin, 12);
        }
    }

}