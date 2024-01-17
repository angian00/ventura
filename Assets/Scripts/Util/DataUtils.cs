
using System;
using System.Collections.Generic;


namespace Ventura.Util
{
    public class DataUtils
    {
        public static int CountValues<T>(T[,] arr2d, T targetValue)
        {
            int res = 0;

            foreach (var arrElem in arr2d)
            {
                if (arrElem.Equals(targetValue))
                    res++;
            }

            return res;
        }

        public static int[] IntValueStats(int[,] arr2d, int maxValue)
        {
            var statsArr = new int[maxValue + 1];

            foreach (var arrElem in arr2d)
            {
                statsArr[arrElem]++;
            }

            return statsArr;
        }

        public static int CountMatchingNeighbours<T>(T[,] arr2d, T targetValue, int x, int y, int radius = 1)
        {
            int nMatches = 0;

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var currX = x + dx;
                    var currY = y + dy;

                    if ((currX == x) && (currY == y))
                        continue;

                    if ((currX < 0) || (currX >= arr2d.GetLength(0)) || (currY < 0) || (currY >= arr2d.GetLength(1)))
                        continue;


                    if (targetValue.Equals(arr2d[currX, currY]))
                        nMatches++;

                }
            }

            return nMatches;
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
