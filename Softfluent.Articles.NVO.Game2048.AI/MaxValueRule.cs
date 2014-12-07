using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class MaxValueRule : AIRule
    {
        internal MaxValueRule() { }
        public MaxValueRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            return Coefficient * grid.Max(gridSideSize);
        }

        public override AIUse Usecase { get { return AIUse.AfterMoveGrid; } }
    }
}
