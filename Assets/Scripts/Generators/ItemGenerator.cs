
using System.Collections.Generic;
using Ventura.GameLogic;
using Ventura.Util;
using Random = UnityEngine.Random;

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
        public List<BookItem> GenerateBooks(int nItems = 10)
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
            var amount = Random.Range(1, 50);

            var title = _titleGen.GenerateString(new Dictionary<string, string>() { { "skill", DataUtils.EnumToStr<SkillId>(skill) } });
            string author = null;
            if (DataUtils.RandomBool())
                author = FileStringGenerator.FirstNames.GenerateString();

            return new BookItem(title, author, skill, amount);
        }
    }

}