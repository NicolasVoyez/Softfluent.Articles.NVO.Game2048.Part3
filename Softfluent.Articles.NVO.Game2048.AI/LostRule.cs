using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    [NotOptimizable]
    public class LostRule : AIRule
    {
        internal LostRule() { }
        private static readonly double _LoosingScore = -2000000;
        public LostRule(double coefficient)
        {
            Coefficient = coefficient;
        }

        public override double Coefficient
        {
            get;
            set;
        }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            return Coefficient *( grid.IsLost(gridSideSize) ? _LoosingScore : 0);
        }

        public override AIUse Usecase { get { return AIUse.WorstCaseGrid; } }
    }
}
