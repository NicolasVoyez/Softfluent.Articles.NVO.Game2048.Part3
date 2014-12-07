using System;
using System.Xml.Serialization;
using Softfluent.Articles.NVO.Game2048.AI.ModifiedVersions;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class PowedSmoothnessRule : AIRule, IDoubleBasedEvolution
    {
        internal PowedSmoothnessRule() { }

        public PowedSmoothnessRule(double coefficient)
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
                        smoothness -= Math.Abs(Math.Pow(currCell, _PowValue) - Math.Pow(cellX, _PowValue));

                    var cellY = grid.GetNextValueWithVector(x, y, 0, 1, gridSideSize);
                    if (currCell != cellY && cellY != 0)
                        smoothness -= Math.Abs(Math.Pow(currCell, _PowValue) - Math.Pow(cellY, _PowValue));
                }
            });
            return smoothness * Coefficient;
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
