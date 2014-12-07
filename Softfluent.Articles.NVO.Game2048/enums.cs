using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048
{
    public enum GameResult
    {
        Win, 
        Loss,
        Continue
    }

    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
    }

    public static class Directions
    {
        public static IEnumerable<Direction> All
        {
            get
            {
                yield return Direction.Top;
                yield return Direction.Left;
                yield return Direction.Bottom;
                yield return Direction.Right;
            }
        }
    }
}
