
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Ventura.Util
{
    public class DataUtils
    {
        public static int CountValues<T>(T[,] arr, T targetItem)
        {
            int res = 0;

            foreach (var item in arr)
            {
                if (item.Equals(targetItem))
                    res++;
            }

            return res;
        }


        public static T ChooseWeighted<T>(List<T> values, List<int> weights)
        {
            var cumWeights = new List<int>();
            int currCum = 0;
            foreach (var currWeight in weights)
            {
                currCum += currWeight;
                cumWeights.Add(currCum);
            }

            int chosenCumWeight = Random.Range(0, currCum + 1);

            var chosenIndex = cumWeights.BinarySearch(chosenCumWeight);
            if (chosenIndex < 0)
            {
                //as per List.BinarySearch docs: the complementof the next index is returned
                chosenIndex = ~chosenIndex;
            }

            return values[chosenIndex];
        }


        public static bool RandomBool()
        {
            if (Random.value >= 0.5)
                return true;
            return false;
        }
     
        public static string EnumToStr<T>(T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }

        public static IEnumerable<T> EnumValues<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }

    }

}
