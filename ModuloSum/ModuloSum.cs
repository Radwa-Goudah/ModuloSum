using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class ModuloSum
    {
        #region YOUR CODE IS HERE    

        #region FUNCTION#1: Calculate the Value
        static int item;
        static int rest_x;
        static bool[,] dp_Arr;
        static bool rest = false;
        //Your Code is Here:
        //==================
        /// <summary>
        /// Fill this function to check whether there's a subsequence numbers in the given array that their sum is divisible by M
        /// </summary>
        /// <param name="items">array of integers</param>
        /// <param name="N">array size </param>
        /// <param name="M">value to check against it</param>
        /// <returns>true if there's subsequence with sum divisible by M... false otherwise</returns>
        static public bool SolveValue(int[] items, int N, int M)
        {
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();
            dp_Arr = new bool[N + 1, M];
            dp_Arr[0, 0] = true;
            for (int indix = 1; indix <= N; indix++)
            {
                item = items[indix - 1] % M;
                dp_Arr[indix, item] = true;
                if (item == 0)
                {
                    rest_x = items[indix - 1];
                    rest = true;
                    return true;
                }
                else if (item > 0 && N == 1) 
                {
                    dp_Arr[N, 0] = false;
                    break;
                }
                if (indix == 1)
                {
                    continue;
                }
                for (int cross = 0; cross < M; cross++)
                {
                    if(dp_Arr[indix - 1, cross] == true||dp_Arr[indix-1, (cross - item + M) % M] == true)  
                    {
                        dp_Arr[indix, cross] = true;
                    }
                }                
            }
        return dp_Arr[N, 0];
        }
        #endregion
        #region FUNCTION#2: Construct the Solution
        //Your Code is Here:
        //==================
        /// <returns>if exists, return the numbers themselves whose sum is divisible by ‘M’ else, return null</returns>
        static public int[] ConstructSolution(int[] items, int N, int M)
        {
            //REMOVE THIS LINE BEFORE START CODING
            //throw new NotImplementedException();
            if (rest == true)
            {
                rest = false;
                int[] rest_arr = new int[1];
                rest_arr[0] = rest_x;
                return rest_arr;
            }
            else if (dp_Arr[N, 0] == false)
            {
                return null;
            }
            else
            {
                int cross = 0;
                List<int> result = new List<int>();
                for (int indix = N; indix > 0; indix--) 
                {
                    item = items[indix - 1] % M;
                    if ((cross == item && dp_Arr[indix, cross] == true)||(dp_Arr[indix, cross] == true && dp_Arr[indix - 1, (cross - item + M) % M] == true)) 
                    {
                        result.Add(items[indix-1]);
                        cross = (cross - item + M) % M;                   
                    }

                }
                return result.ToArray();
            }
        }
        #endregion
        #endregion

    }
}
