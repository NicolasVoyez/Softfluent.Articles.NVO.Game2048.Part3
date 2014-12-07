using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class EmptyCellsRule : AIRule
    {

        internal EmptyCellsRule() { }
        public EmptyCellsRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            int emptyCellsCount = 0;
            grid.Foreach(gridSideSize, (x, y, content) =>
            {
                if (content == 0)
                    emptyCellsCount++;
            });
            return Coefficient * emptyCellsCount;
        }

        public override AIUse Usecase { get { return AIUse.AfterMoveGrid; } }
    }
}
