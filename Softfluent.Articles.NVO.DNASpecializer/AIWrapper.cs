using Softfluent.Articles.NVO.Game2048.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.DNASpecializer
{
    public class AIWrapper : IComparable, IComparable<AIWrapper>
    {
        public static int TestsToDo = 200;

        public int Wins { get; set; }
        public int Score { get; set; }
        public AI Intelligence { get; set; }

        public string[] XmlTypes
        {
            get
            {
                return Types.Select(x => x.AssemblyQualifiedName).ToArray();
            }
            set
            {
                Types = (from typeName in value
                         select Type.GetType(typeName)).ToArray();
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public Type[] Types { get; set; }

        internal AIWrapper()
        {

        }

        public AIWrapper(AI intel)
        {
            Intelligence = intel;

            Types = intel.Rules.Select(x => x.GetType()).ToArray();
            GenerateScore();
        }

        public void GenerateScore()
        {

            var totalScore = 0;
            var totalWins = 0;
            Parallel.For(0, TestsToDo, (i) =>
            {
                Game2048.Game2048 game = new Game2048.Game2048();
                var result = Intelligence.PlayGame(game);

                if (result.Item1 == Game2048.GameResult.Win)
                    System.Threading.Interlocked.Increment(ref totalWins);
                System.Threading.Interlocked.Add(ref totalScore, game.Score);
            });
            Score = totalScore;
            Wins = totalWins;
        }

        public double PercentWin
        {
            get { return Math.Round((double)Wins * 100 / (double)TestsToDo, 2); }
        }

        public override string ToString()
        {
            return string.Format("#{3} : {0}% win{2} & {1} pts", double.IsNaN(PercentWin) ? 0 : PercentWin, Score, PercentWin > 1 ? "s" : "", GetHashCode());
        }

        public static bool operator >(AIWrapper res1, AIWrapper res2)
        {
            return MoreThan5PercentDifference(res1, res2) || (LessThan5PercentDifference(res2, res1) && res1.Score > res2.Score);
        }

        public static bool operator <(AIWrapper res1, AIWrapper res2)
        {
            return MoreThan5PercentDifference(res1, res2) || (LessThan5PercentDifference(res2, res1) && res1.Score < res2.Score);
        }

        public static bool operator ==(AIWrapper res1, AIWrapper res2)
        {
            if (object.ReferenceEquals(res1, null))
                return object.ReferenceEquals(res2, null);
            else if (object.ReferenceEquals(res2, null))
                return false;
            
            return ((int)res1.PercentWin == (int)res2.PercentWin) && Math.Abs(res1.Score - res2.Score) < double.Epsilon;
        }

        public static bool operator !=(AIWrapper res1, AIWrapper res2)
        {
            return !(res1 == res2);
        }

        public static bool operator >=(AIWrapper res1, AIWrapper res2)
        {
            return MoreThan5PercentDifference(res1, res2) || (LessThan5PercentDifference(res2, res1) && res1.Score >= res2.Score);
        }

        public static bool operator <=(AIWrapper res1, AIWrapper res2)
        {
            return MoreThan5PercentDifference(res1, res2) || (LessThan5PercentDifference(res2, res1) && res1.Score <= res2.Score);
        }

        private static bool MoreThan5PercentDifference(AIWrapper res1, AIWrapper res2)
        {
            return res1.PercentWin - res2.PercentWin > 5;
        }
        private static bool LessThan5PercentDifference(AIWrapper res1, AIWrapper res2)
        {
            return res1.PercentWin - res2.PercentWin < 5;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as AIWrapper);
        }

        public int CompareTo(AIWrapper other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }
            else if (this > other)
                return 1;
            else if (this == other)
                return 0;
            return -1;
        }
    }
}
