using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public static class Extensions
    {
        public static int GetNextValueWithVector(this int[,] board, int startx, int starty, int vectorx, int vectory, int size)
        {
            for (int x = startx + vectorx; x < size; x += vectorx)
            {
                if (x < 0)
                    return 0;
                
                for (int y = starty + vectory; y < size; y += vectory)
                {
                    if (y < 0)
                        return 0;

                    var cell = board[x,y];
                    if (cell != 0)
                        return cell;

                    if (vectory == 0)
                        y = size; // fake break;
                }

                if (vectorx == 0)
                    return 0;
            }

            return 0;
        }
    }


}
