using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class MonotonyRule : AIRule
    {
        internal MonotonyRule() { }
        public MonotonyRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            var totals = new double[] { 0, 0, 0, 0 };

            grid.Foreach(gridSideSize, (x, y, content) =>
            {
                if (content == 0) return;

                if (y < 3)
                {
                    int nextY = y;
                    int next;
                    do
                    {
                        nextY++;
                        next = grid[x, nextY];
                    }
                    while (nextY < gridSideSize - 1 && next == 0);

                    if (content > next)
                        totals[0] -= content - next;
                    else
                        totals[1] -= next - content;
                }
                if (x < 3)
                {
                    int nextX = x;
                    int next;
                    do
                    {
                        nextX++;
                        next = grid[nextX, y];
                    }
                    while (nextX < gridSideSize - 1 && next == 0);

                    if (content > next)
                        totals[2] -= content - next;
                    else
                        totals[3] -= next - content;
                }
            });

            return Coefficient * (Math.Min(totals[0], totals[1]) + Math.Min(totals[2], totals[3]));
        }

        public override AIUse Usecase { get { return AIUse.WorstCaseGrid; } }

    }
}