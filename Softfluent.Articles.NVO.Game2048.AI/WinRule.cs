using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    [NotOptimizable]
    public class WinRule : AIRule
    {
        internal WinRule() { }
        private static readonly double _WiningScore = 2000000;
        public WinRule(double coefficient)
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
            return Coefficient * (grid.Any(Game2048.SIZE, Game2048.SIZE, (content) => content == Game2048.WIN) ? _WiningScore : 0);
        }

        public override AIUse Usecase { get { return AIUse.AfterMoveGrid; } }
    }
}
