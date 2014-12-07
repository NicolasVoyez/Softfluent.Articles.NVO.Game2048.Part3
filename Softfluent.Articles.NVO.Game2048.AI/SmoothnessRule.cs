using System;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class SmoothnessRule : AIRule
    {
        internal SmoothnessRule() { }
        public SmoothnessRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            double smoothness = 0;

            grid.Foreach(gridSideSize, (x, y) =>
            {
                var currCell = grid[x, y];
                if (currCell != 0)
                {
                    var cellX = grid.GetNextValueWithVector(x, y, 1, 0, gridSideSize);
                    if (currCell != cellX && cellX != 0)
                        smoothness -= Math.Abs(currCell - cellX);

                    var cellY = grid.GetNextValueWithVector(x, y, 0, 1, gridSideSize);
                    if (currCell != cellY && cellY != 0)
                        smoothness -= Math.Abs( currCell - cellY);
                }
            });
            return smoothness * Coefficient;
        }

        public override AIUse Usecase { get { return AIUse.WorstCaseGrid; } }

    }
}
