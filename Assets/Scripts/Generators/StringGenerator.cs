using System.Collections.Generic;
using UnityEngine;
using Ventura.Util;

namespace Ventura.Generators
{
    public interface StringGenerator
    {
        public string GenerateString();
    }


    public class DummyStringGenerator : StringGenerator
    {
        private static DummyStringGenerator _instance = new DummyStringGenerator();
        public static DummyStringGenerator Instance { get => _instance; }

        private char _currChar = 'A';

        public string GenerateString()
        {
            var res = "";

            for (int i = 0; i < 6; i++)
            {
                res += _currChar.ToString();
            }

            _currChar++;

            return res;
        }
    }


    public class FileStringGenerator : StringGenerator
    {
        private List<string> _names = new();
        private List<int> _frequencies = new();


        //public static FileStringGenerator Sites = new FileStringGenerator("names_sites_international");
        public static FileStringGenerator Sites = new FileStringGenerator("names_sites_italia");
        public static FileStringGenerator FirstNames = new FileStringGenerator("names_people_toscana");


        protected FileStringGenerator(string sourceFile) : this(new string[] { sourceFile }) { }

        protected FileStringGenerator(string[] sourceFiles)
        {
            foreach (var file in sourceFiles)
                loadFile(file);
        }


        private void loadFile(string filename)
        {
            var fileObj = Resources.Load<TextAsset>($"Data/{filename}");
            var fileLines = fileObj.text.Split("\n");


            foreach (var line in fileLines)
            {
                if (line.Length == 0 || line[0] == '#' || line.Trim() == "")
                    //skip comments and empty lines
                    continue;

                var tokens = line.Trim().Split("|");
                Debug.Assert(tokens.Length <= 2);

                var name = tokens[0];
                var freq = (tokens.Length == 1 ? 1 : int.Parse(tokens[1]));

                _names.Add(name);
                _frequencies.Add(freq);
            }
        }


        public string GenerateString()
        {
            return RandomUtils.RandomWeighted(_names, _frequencies);
        }
    }

}