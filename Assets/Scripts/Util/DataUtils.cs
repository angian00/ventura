
using UnityEngine;

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
    }
}
