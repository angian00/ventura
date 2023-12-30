using UnityEngine;
using System.Collections.Generic;
using Ventura.Util;

namespace Ventura.Generators
{
    public class DummyNameGenerator
    {
        private static char _currChar = 'A';

        public static string GenerateName()
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


    public class NameGenerator
    {
        private List<string> _names = new();
        private List<int> _frequencies = new();


        //public static NameGenerator Sites = new NameGenerator("names_sites_international");
        public static NameGenerator Sites = new NameGenerator("names_sites_italia");


        private NameGenerator(string sourceFile)
        {
            loadNameFile(sourceFile);
        }


        private NameGenerator(string[] sourceFiles)
        {
            foreach (var file in sourceFiles)
                loadNameFile(file);
        }


        private void loadNameFile(string filename)
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
                var freq = ( tokens.Length == 1 ? 1 : int.Parse(tokens[1]) );

                _names.Add(name);
                _frequencies.Add(freq);
            }
        }


        public string GenerateName()
        {
            return DataUtils.ChooseWeighted(_names, _frequencies);
        }
    }

}