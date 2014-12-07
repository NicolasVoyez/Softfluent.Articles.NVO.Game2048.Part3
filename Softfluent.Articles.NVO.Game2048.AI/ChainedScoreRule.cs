using Softfluent.Articles.NVO.Game2048.AI.ModifiedVersions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Softfluent.Articles.NVO.Game2048.AI
{
    public class ChainedScoreRule : AIRule, IDoubleBasedEvolution
    {
        internal ChainedScoreRule()
        {

        }
        public ChainedScoreRule(double coefficient)
        {
            Coefficient = coefficient;
        }
        public double CurrentEvolutionState
        {
            get;
            set;
        }

        [XmlIgnore]
        public int EvolutionMaxRange
        {
            get { return 10; }
        }

        [XmlIgnore]
        public int EvolutionMinRange
        {
            get { return 1; }
        }

        public void Evolve(double newEvolutionValue = 0)
        {
            if (newEvolutionValue >= EvolutionMinRange && newEvolutionValue <= EvolutionMaxRange)
                CurrentEvolutionState = newEvolutionValue;
            else
                CurrentEvolutionState = _random.NextDouble() * (EvolutionMaxRange - EvolutionMinRange) + EvolutionMinRange;
        }

        public override AIUse Usecase
        {
            get { return AIUse.AfterMoveGrid; }
        }

        public override double Coefficient
        {
            get;
            set;
        }

        public override double CalculatePoints(int[,] grid, Direction fromDirection, int gridSideSize)
        {
            List<PositionAndLenght> nextPointsAndDistance = new List<PositionAndLenght>();
            int maxValue = 2;
            grid.Foreach(gridSideSize,(x,y,content)=> {
                if (content < maxValue )
                    return;
                if (content > maxValue)
                {
                    maxValue = content;
                    nextPointsAndDistance.Clear();
                }
                if (content == maxValue)
                    nextPointsAndDistance.Add( new PositionAndLenght(x, y, content, 0));
            });
            while (nextPointsAndDistance.Any( x => !x.Ended ))
            {
                List<PositionAndLenght> newPositions = new List<PositionAndLenght>();
                for (int i = nextPointsAndDistance.Count -1; i>=0; i--) 
                {
                    var nextFound = false;
                    var point = nextPointsAndDistance[i];
                    if (point.Value == 1)
                        point.Ended = true;
                    if (point.Ended)
                        continue;

                    if (point.X < gridSideSize - 1 && grid[point.X + 1, point.Y] == point.Value - 1)
                    {
                        nextPointsAndDistance.Add(new PositionAndLenght(point.X + 1, point.Y, point.Value - 1, point.Distance + 1));
                        nextFound = true;
                    }
                    if (point.X > 0 && grid[point.X - 1, point.Y] == point.Value - 1)
                    {
                        nextPointsAndDistance.Add(new PositionAndLenght(point.X - 1, point.Y, point.Value - 1, point.Distance + 1));
                        nextFound = true;
                    }
                    if (point.Y < gridSideSize - 1 && grid[point.X , point.Y +1] == point.Value - 1)
                    {
                        nextPointsAndDistance.Add(new PositionAndLenght(point.X, point.Y +1, point.Value - 1, point.Distance + 1));
                        nextFound = true;
                    }
                    if (point.Y > 0 && grid[point.X , point.Y -1] == point.Value - 1)
                    {
                        nextPointsAndDistance.Add(new PositionAndLenght(point.X, point.Y -1, point.Value - 1, point.Distance + 1));
                        nextFound = true;
                    }
                    if (nextFound)
                        nextPointsAndDistance.Remove(point);
                    else
                        point.Ended = true;
                }
            }
            if (nextPointsAndDistance.Count == 0)
                return 0;
            return Coefficient * Math.Pow(nextPointsAndDistance.Select(x => x.Distance).Max(),CurrentEvolutionState );

        }

        private class PositionAndLenght
        {
            public PositionAndLenght(int x, int y, int val, int distance)
            {
                X = x;
                Y = y;
                Value = val;
                Distance = distance;
                Ended = false;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Value { get; set; }
            public int Distance { get; set; }
            public bool Ended { get; set; }
        }


        public override string ToString()
        {
            return base.ToString() + " (" + Math.Round(CurrentEvolutionState, 3) + " pow version)";
        }
    }
}
