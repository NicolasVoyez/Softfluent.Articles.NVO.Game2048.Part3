using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace Softfluent.Articles.NVO.Game2048.AI
{

    public class AI
    {
        [System.Xml.Serialization.XmlElement("EmptyCellsRule", typeof(EmptyCellsRule))]
        [System.Xml.Serialization.XmlElement("LostRule", typeof(LostRule))]
        [System.Xml.Serialization.XmlElement("MaxValueRule", typeof(MaxValueRule))]
        [System.Xml.Serialization.XmlElement("MonotonyRule", typeof(MonotonyRule))]
        [System.Xml.Serialization.XmlElement("RandomRule", typeof(RandomRule))]
        [System.Xml.Serialization.XmlElement("SmoothnessRule", typeof(SmoothnessRule))]
        [System.Xml.Serialization.XmlElement("SpamTopRightTillReflexionNeeded", typeof(SpamTopRightTillReflexionNeeded))]
        public List<AIRule> Rules { get; set; }

        [XmlIgnore]
        private IEnumerable<AIRule> AfterMoveRules
        {
            get { return Rules.Where(x => x.Usecase == AIUse.AfterMoveGrid); }
        }
        [XmlIgnore]
        private IEnumerable<AIRule> WorstCaseRules
        {
            get { return Rules.Where(x => x.Usecase == AIUse.WorstCaseGrid); }
        }
        internal AI()
        {

        }

        public AI(IEnumerable<AIRule> rules)
        {
            Rules = new List<AIRule>(rules);
        }

        private Direction GetBestDirection(Game2048 game)
        {
            Direction bestDirection = Direction.Right;
            double bestScore = double.MinValue;

            foreach (var direction in Directions.All)
            {
                var gameAfterMove = game.TryDirection(direction);

                // verify no move is forbidden
                if (!game.WorkBoard.IsDifferent(gameAfterMove, Game2048.SIZE, Game2048.SIZE))
                    continue;

                double afterMoveScore = AfterMoveRules.Sum(rule => rule.CalculatePoints(gameAfterMove, direction, Game2048.SIZE));

                double worstCaseScore = double.MaxValue;
                // after moving the gameboard, let's check what new item position is the worst possible, and use this score.
                gameAfterMove.Foreach(Game2048.SIZE, (x, y, content) =>
                {
                    if (content != 0) return;

                    foreach (int possibleRandomValue in Game2048.DISTINCT_RANDOM_ITEMS)
                    {
                        gameAfterMove[x, y] = possibleRandomValue;
                        double possibleScore = WorstCaseRules.Sum(rule => rule.CalculatePoints(gameAfterMove, direction, Game2048.SIZE));

                        if (possibleScore < worstCaseScore)
                            worstCaseScore = possibleScore;

                        gameAfterMove[x, y] = 0;
                    }
                });

                double score = worstCaseScore + afterMoveScore;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestDirection = direction;
                }
            }
            return bestDirection;
        }

        public GameResult PlayGame()
        {
            Game2048 game = new Game2048();
            return PlayGame(game, null).Item1;
        }
        public Tuple<GameResult, int[,]> PlayGame(Game2048 game, Action onPlayed = null)
        {
            GameResult result = GameResult.Continue;
            while (result == GameResult.Continue)
            {
                result = game.DoAction(GetBestDirection(game));
                if (onPlayed != null)
                    onPlayed();
            }
            return new Tuple<GameResult, int[,]>(result, game.WorkBoard);
        }
    }
}
