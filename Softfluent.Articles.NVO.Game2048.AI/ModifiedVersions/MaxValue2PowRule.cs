using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Softfluent.Articles.NVO.Game2048.AI.ModifiedVersions;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class MaxValue2PowRule : AIRule, IDoubleBasedEvolution
    {
        internal MaxValue2PowRule()
        {

        }

        public MaxValue2PowRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient { get; set; }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {

            var max = grid.Max(gridSideSize);
            return max == 0 ? 0 : Coefficient * Math.Pow(_PowValue, max);
        }

        private double _PowValue = 2;

        public override string ToString()
        {
            return base.ToString() + " (" + Math.Round(_PowValue, 3) + " pow version)";
        }

        public double CurrentEvolutionState { get { return _PowValue; }  set { _PowValue = value; } }

        [XmlIgnore]
        public int EvolutionMaxRange {get { return 10; }}

        [XmlIgnore]
        public int EvolutionMinRange { get { return 1; } }

        public void Evolve(double newEvolutionValue = 0.0d)
        {
            if (newEvolutionValue >= EvolutionMinRange && newEvolutionValue <= EvolutionMaxRange)
                CurrentEvolutionState = newEvolutionValue;
            else
                CurrentEvolutionState = _random.NextDouble()*(EvolutionMaxRange - EvolutionMinRange) + EvolutionMinRange;
        }

        public override AIUse Usecase { get { return AIUse.AfterMoveGrid; } }
    }
}
