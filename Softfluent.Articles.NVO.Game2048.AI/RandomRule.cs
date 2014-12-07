using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class RandomRule : AIRule
    {

        internal RandomRule() { }
        private static Random _r = new Random();

        public RandomRule(double coefficient)
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
            return (_r.Next(0,600)-300)/100d;
        }

        public override AIUse Usecase { get { return AIUse.AfterMoveGrid; } }
    }
}
