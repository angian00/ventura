
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Ventura.GameLogic;
using Ventura.Util;

namespace Ventura.Generators
{
    public class BookItemGenerator
    {
        private static BookItemGenerator _instance = new BookItemGenerator();
        public static BookItemGenerator Instance { get => _instance; }

        private TemplatedStringGenerator _titleGen;

        private BookItemGenerator()
        {
            _titleGen = new TemplatedStringGenerator("language_book_titles");
        }

        //public void GenerateBooks(Actor actor, int nItems=10)
        public List<BookItem> GenerateBooks(int nItems=10)
        {
            var res = new List<BookItem>();

            for (var i = 0; i < nItems; i++)
            {
                res.Add(GenerateBookItem());
            }

            return res;
        }


        private BookItem GenerateBookItem()
        {
            var skill = SkillId.Latin;
            var title = _titleGen.GenerateString(new Dictionary<string, string>() { { "skill", DataUtils.EnumToStr<SkillId>(skill)} });
            var amount = Random.Range(1, 50);

            return new BookItem(title, skill, amount);
        }
    }

}