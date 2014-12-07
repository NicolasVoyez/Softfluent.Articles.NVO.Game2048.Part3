using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class PowedMonotonyRule : AIRule
    {
        internal PowedMonotonyRule() { }
        public PowedMonotonyRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            var totals = new double[] { 0, 0, 0, 0 };
            
            grid.Foreach(gridSideSize, (x, y, content) =>
            {
                if (content == 0)
                    return;
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
                        totals[0] -= Math.Pow(content, _PowValue) - Math.Pow(next, _PowValue);
                    else
                        totals[1] -= Math.Pow(next, _PowValue) - Math.Pow(content, _PowValue);
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
                    while (nextX < gridSideSize-1 && next == 0);

                    if (content > next)
                        totals[2] -= Math.Pow(content, _PowValue) - Math.Pow(next, _PowValue);
                    else
                        totals[3] -= Math.Pow(next, _PowValue) - Math.Pow(content, _PowValue);
                }
            });

            return Coefficient * (Math.Min(totals[0], totals[1]) + Math.Min(totals[2], totals[3]));


        }

        private double _PowValue = 2;

        public override string ToString()
        {
            return base.ToString() + " (" + Math.Round(_PowValue, 3) + " pow version)";
        }

        public double CurrentEvolutionState { get { return _PowValue; }  set { _PowValue = value; } }

        [XmlIgnore]
        public int EvolutionMaxRange { get { return 6; } }

        [XmlIgnore]
        public int EvolutionMinRange { get { return 1; } }

        public void Evolve(double newEvolutionValue = 0.0d)
        {
            if (newEvolutionValue >= EvolutionMinRange && newEvolutionValue <= EvolutionMaxRange)
                CurrentEvolutionState = newEvolutionValue;
            else
                CurrentEvolutionState = _random.NextDouble() * (EvolutionMaxRange - EvolutionMinRange) + EvolutionMinRange;
        }

        public override AIUse Usecase { get { return AIUse.WorstCaseGrid; } }

    }
}