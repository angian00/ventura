using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Random = UnityEngine.Random;


namespace Ventura.Util
{
    public class RandomUtils
    {
        public static T RandomWeighted<T>(List<T> values, List<int> weights)
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


        public static Vector2Int RandomEmptyPos(GameMap targetMap)
        {
            //choose starting lastPos on a empty square
            while (true)
            {
                int x = Random.Range(0, targetMap.Width);
                int y = Random.Range(0, targetMap.Height);
                if (targetMap.IsEmpty(x, y))
                    return new Vector2Int(x, y);
            }
        }

        public static Vector2Int RandomWalkablePos(GameMap targetMap)
        {
            while (true)
            {
                int x = Random.Range(0, targetMap.Width);
                int y = Random.Range(0, targetMap.Height);
                if (targetMap.IsWalkable(x, y))
                    return new Vector2Int(x, y);
            }

        }
        public static bool RandomBool()
        {
            if (Random.value >= 0.5)
                return true;
            return false;
        }

        public static Vector2Int RandomMovement()
        {
            while (true)
            {
                int deltaX = Random.Range(-1, 2);
                int deltaY = Random.Range(-1, 2);
                if (deltaX != 0 || deltaY != 0)
                    return new Vector2Int(deltaX, deltaY);
            }
        }
    }

}
