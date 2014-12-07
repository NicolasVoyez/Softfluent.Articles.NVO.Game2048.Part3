using Softfluent.Articles.NVO.Game2048.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<AIRule> toLaunch = new List<AIRule>();
            var all = args.Contains("-all");
            if (all || args.Contains("-smooth"))
                toLaunch.Add(new SmoothnessRule(1));
            if (all || args.Contains("-monotony"))
                toLaunch.Add(new MonotonyRule(1));
            if (all || args.Contains("-maxvalue"))
                toLaunch.Add(new MaxValueRule(1));
            if (all || args.Contains("-empty"))
                toLaunch.Add(new EmptyCellsRule(1));
            if (all || args.Contains("-lost"))
                toLaunch.Add(new LostRule(1d));
            if (all || args.Contains("-win"))
                toLaunch.Add(new WinRule(1d));
            if (!args.Contains("-nospam") && (all || args.Contains("-spam")))
                toLaunch.Add(new SpamTopRightTillReflexionNeeded(1));
            if (!args.Contains("-norandom") && (all || args.Contains("-random")))
                toLaunch.Add(new RandomRule(1));
            
            if (toLaunch.Count == 0)
                PlayerMain();
            else if (args.Contains("-stats"))
                PlayIAStatsMain(toLaunch.ToArray());
            else
                PlayIAMain(toLaunch.ToArray());

            Console.ReadKey();

        }

        private static void PlayerMain()
        {
            Game2048 game = new Game2048();

            bool isFinished = false;
            while (!isFinished)
            {
                ShowGrid(game);

                var key = Console.ReadKey();
                GameResult result = GameResult.Continue;
                if (key.Key == ConsoleKey.UpArrow)
                    result = game.DoAction(Direction.Top);
                else if (key.Key == ConsoleKey.DownArrow)
                    result = game.DoAction(Direction.Bottom);
                else if (key.Key == ConsoleKey.RightArrow)
                    result = game.DoAction(Direction.Right);
                else if (key.Key == ConsoleKey.LeftArrow)
                    result = game.DoAction(Direction.Left);



                if (result == GameResult.Loss)
                {
                    isFinished = true;
                    ShowGrid(game);
                    Console.WriteLine();
                    Console.WriteLine("You loose, that's kind of tragic !");
                    Console.WriteLine();
                    Console.ReadKey();
                }
                else if (result == GameResult.Win)
                {
                    isFinished = true;
                    ShowGrid(game);
                    Console.WriteLine();
                    Console.WriteLine("Congratulations, you won !");
                    Console.WriteLine();
                }

            }

            Console.WriteLine("Press Escape to leave !");
            var nowKey = ConsoleKey.M;
            while (nowKey != ConsoleKey.Escape)
                nowKey = Console.ReadKey().Key;
        }

        private static void PlayIAStatsMain(params AIRule[] rules)
        {
            string text = string.Format("Running 1000 tests with selected AI : {0}",
                rules.Aggregate("", (x, y) => x + y.GetType().Name + " (" + y.Coefficient + "),"));
            Console.WriteLine(text);

            List<GameResult> results = new List<GameResult>();
            List<int> scores = new List<int>();
            List<int> bests = new List<int>();
            AI.AI ai = new AI.AI(rules);
            Parallel.For(0, 1000, (i) =>
            {
                Game2048 game = new Game2048();
                var res = ai.PlayGame(game, () =>
                {
                });
                results.Add(res.Item1);
                scores.Add(game.Score);
                bests.Add(game.ShowBoard.Max(Game2048.SIZE));
            });

            Console.WriteLine("Results : ");

            var wins = results.Count(x => x == GameResult.Win);
            Console.WriteLine(" - {0} win and {1} losses.", wins, 1000 - wins);
            Console.WriteLine(" - {0} average score, {1} best, {2} worst.", scores.Average(), scores.Max(), scores.Min());
            Console.WriteLine(" - {0} average best tile, {1} best, {2} worst.", bests.Average(), bests.Max(), bests.Min());

            Console.WriteLine();
            Console.WriteLine();

        }

        private static void PlayIAMain(params AIRule[] ias)
        {

            Game2048 game = new Game2048();
            AI.AI ia = new AI.AI(ias);

            ShowGrid(game);

            var result = ia.PlayGame(game, () =>
            {
                ShowGrid(game);
                System.Threading.Thread.Sleep(75);
            }).Item1;



            if (result == GameResult.Loss)
            {
                ShowGrid(game);
                Console.WriteLine();
                Console.WriteLine("You loose you dumb !");
                Console.WriteLine();
                Console.ReadKey();
            }
            else if (result == GameResult.Win)
            {
                ShowGrid(game);
                Console.WriteLine();
                Console.WriteLine("Congratulations, you won .... A COCONUT !");
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to leave !");
        }

        private static void ShowGrid(Game2048 game)
        {
            Welcome();

            for (int i = 0; i < Game2048.SIZE; i++)
                Console.Write("-------");
            Console.WriteLine("-");
            for (int x = 0; x < Game2048.SIZE; x++)
            {
                Console.Write("|");
                for (int y = 0; y < Game2048.SIZE; y++)
                {
                    var current = game.ShowBoard[x, y].ToString();
                    Console.ForegroundColor = GetColor(current);
                    for (int i = current.Length; i < 6; i++)
                    {
                        current = ((i % 2 == 0) ? " " : "") + current + ((i % 2 == 1) ? " " : "");
                    }

                    Console.Write(current);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("|");
                }
                Console.WriteLine();
                for (int i = 0; i < Game2048.SIZE; i++)
                    Console.Write("-------");
                Console.WriteLine("-");
            }
        }

        private static void Welcome()
        {
            Console.Clear();
            Console.WriteLine("Welcome to 2048. Play with arrows.");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static ConsoleColor GetColor(string current)
        {
            switch (current)
            {
                case "0":
                    return ConsoleColor.Black;
                case "8":
                    return ConsoleColor.Cyan;
                case "16":
                    return ConsoleColor.Blue;
                case "32":
                    return ConsoleColor.DarkBlue;
                case "64":
                    return ConsoleColor.DarkGreen;
                case "128":
                    return ConsoleColor.Green;
                case "256":
                    return ConsoleColor.Yellow;
                case "512":
                    return ConsoleColor.Red;
                case "1024":
                    return ConsoleColor.DarkRed;
                case "2048":
                    return ConsoleColor.DarkMagenta;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
