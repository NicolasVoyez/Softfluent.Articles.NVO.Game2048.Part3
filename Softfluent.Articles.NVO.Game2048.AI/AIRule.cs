using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public abstract class AIRule
    {
        protected static Random _random = new Random();
        public abstract AIUse Usecase { get; }
        public abstract double Coefficient { get; set; }
        public string TypeName { get { return GetType().Name; } }

        public abstract double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize);

        public override string ToString()
        {
            return TypeName + " : " + Math.Round(Coefficient, 3);
        }
    }

    public enum AIUse
    {
        WorstCaseGrid,
        AfterMoveGrid,
    }

}
