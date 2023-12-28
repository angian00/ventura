using Random = UnityEngine.Random;
using System;


namespace Ventura.Generators
{

    public class NameGenerator
    {
        private static char _currChar = 'A';

        public static string GenerateMapName()
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
}