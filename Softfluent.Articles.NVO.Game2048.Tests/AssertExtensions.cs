using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.Tests
{
    public static class AssertExtensions
    {
        public static void AssertBoardEquals<T>(this T[,] board1, T[,] board2, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Assert.AreEqual(board1[i, j], board2[i, j]);
                }
            }
        }
    }
}
