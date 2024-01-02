
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Ventura.Util
{
    public class DataUtils
    {
        public static int CountValues<T>(T[,] arr2d, T targetElem)
        {
            int res = 0;

            foreach (var arrElem in arr2d)
            {
                if (arrElem.Equals(targetElem))
                    res++;
            }

            return res;
        }


        public static T[] Flatten<T>(T[,] arr2d)
        {
            int w = arr2d.GetLength(0);
            int h = arr2d.GetLength(1);

            T[] arr1d = new T[w * h];

            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                    arr1d[j * w + i] = arr2d[i, j];

            return arr1d;
        }


        public static T[,] Unflatten<T>(T[] arr1d, int w, int h)
        {
            T[,] arr2d = new T[w, h];

            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                    arr2d[i, j] = arr1d[j * w + i];

            return arr2d;
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
