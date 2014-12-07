using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    [NotOptimizable]
    public class SpamTopRightTillReflexionNeeded : AIRule
    {
        internal SpamTopRightTillReflexionNeeded() { }
        public SpamTopRightTillReflexionNeeded(double coefficient)
        {
            Coefficient = coefficient;
        }

        private const int ReflexionPoint = 6;

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            if (!(fromDirection == Direction.Bottom || fromDirection == Direction.Right))
                return 0;

            if (grid.Max(gridSideSize) > ReflexionPoint)
                return 0;

            return ReflexionPoint * Coefficient;
        }

        public override AIUse Usecase { get { return AIUse.AfterMoveGrid; } }
    }
}
