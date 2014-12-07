using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048
{
    public static class ArrayExtension
    {
        public static void Fill<T>(this T[,] array, int x_size, int y_size, T value)
        { 
            for (int x = 0; x < x_size; x++)
            {
                for (int y = 0; y < y_size; y++)
                {
                    array[x, y] = value;
                }
            }
        }
        public static bool Any<T>(this T[,] array, int x_size, int y_size, Predicate<T> condition)
        {
            for (int x = 0; x < x_size; x++)
            {
                for (int y = 0; y < y_size; y++)
                {
                    if (condition(array[x, y]))
                        return true;
                }
            }
            return false;
        }
        public static bool Any<T>(this T[,] array, int x_size, int y_size, Func<T, int, int, bool> condition)
        {
            for (int x = 0; x < x_size; x++)
            {
                for (int y = 0; y < y_size; y++)
                {
                    if (condition(array[x, y], x, y))
                        return true;
                }
            }
            return false;
        }

        public static bool IsDifferent<T>(this T[,] array, T[,] array2, int x_size, int y_size)
        {
            for (int x = 0; x < x_size; x++)
            {
                for (int y = 0; y < y_size; y++)
                {
                    if (!array[x, y].Equals(array2[x, y]))
                        return true;
                }
            }
            return false;
        }

        public static void Foreach<T>(this T[,] array, int arraySideSize, Action<int, int> toDo)
        {
            for (int x = 0; x < arraySideSize ; x++)
            {
                for (int y = 0; y < arraySideSize ; y++)
                    toDo(x, y);
            }
        }
        public static void Foreach<T>(this T[,] array, int arraySideSize, Action<int, int, T> toDo)
        {
            for (int x = 0; x < arraySideSize ; x++)
            {
                for (int y = 0; y < arraySideSize ; y++)
                    toDo(x, y, array[x, y]);
            }
        }

        public static int Max(this int[,] array, int arraySideSize)
        {
            int max = 0;
            array.Foreach(arraySideSize, (x, y, content) =>
            {
                if (content > max)
                    max = content;
            });
            return max;
        }

        public static int Sum(this int[,] array, int arraySideSize)
        {
            int sum = 0;
            array.Foreach(arraySideSize, (x, y, content) =>
            {
                    sum += content;
            });
            return sum;
        }
        public static bool IsLost(this int[,] array, int arraySideSize)
        {

            if (array.Any(arraySideSize, arraySideSize, x => x == 0))
                return false;

            for (int x = 0; x < arraySideSize; x++)
            {
                for (int y = 0; y < arraySideSize; y++)
                {
                    if ((x != arraySideSize - 1 && array[x, y] == array[x + 1, y]) ||
                        (y != arraySideSize - 1 && array[x, y] == array[x, y + 1]))
                        return false;
                }
            }
            return true;
        }
    }
}
