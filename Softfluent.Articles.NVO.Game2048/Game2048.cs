using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048
{
    public class Game2048
    {
        public const int POW = 2;
        public const int SIZE = 4;
        public const int WIN = 11;
        public const int TOTAL_CASES = SIZE * SIZE;

        private static readonly int[] RANDOM_ITEMS = new int[] { 1, 1, 1, 1, 1, 2 };
        public static readonly int[] DISTINCT_RANDOM_ITEMS = new int[] {  1, 2 };

        private int[,] _WorkBoard;
        public int[,] WorkBoard
        {
            get
            {
                return _WorkBoard;
            }
        }

        private Dictionary<int, int> _Pows = new Dictionary<int, int> { { 0, 0 } };

        public int Score
        {
            get 
            {
                int score = 0;
                ShowBoard.Foreach(SIZE, (x, y, content) =>
                {
                    score += content;
                });
                return score;
            }
        }

        public int[,] ShowBoard
        {
            get
            {
                int[,] board = new int[SIZE, SIZE];
                for (int x = 0; x < SIZE; x++)
                {
                    for (int y = 0; y < SIZE; y++)
                    {
                        var current = WorkBoard[x, y];
                        int show;
                        if (!_Pows.TryGetValue(current, out show))
                        {
                            show = (int)Math.Pow(POW, current);
                            _Pows.Add(current, show);
                        }
                        board[x, y] = show;
                    }
                }
                return board;
            }
        }


        private Random _GameRandom = new Random();

        public Game2048()
        {
            InitializeBoard();
        }
        public Game2048(int[,] board)
        {
            _WorkBoard = board;
        }

        private void InitializeBoard()
        {
            _WorkBoard = new int[SIZE, SIZE];
            _WorkBoard.Fill(SIZE, SIZE, 0);

            InsertRandom(RANDOM_ITEMS.Min());
            InsertRandom(RANDOM_ITEMS.Min());

        }

        private void InsertRandom(int fixedRandomValue = -1)
        {

            int item = fixedRandomValue == -1 ? RANDOM_ITEMS[_GameRandom.Next(0, RANDOM_ITEMS.Length)] : fixedRandomValue;
            int position = _GameRandom.Next(0, TOTAL_CASES);
            while (true)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    for (int y = 0; y < SIZE; y++)
                    {
                        int numPos = _WorkBoard[x, y];
                        if (numPos == 0)
                        {
                            if (position == 0)
                            {
                                _WorkBoard[x, y] = item;
                                return;
                            }
                            else
                                position--;
                        }
                    }
                }
            }

        }

        public GameResult DoAction(Direction direction)
        {
            int[,] oldBoard = new int[SIZE, SIZE];
            Array.Copy(_WorkBoard, 0, oldBoard, 0, SIZE * SIZE);
            GoDirection(direction);

            if (_WorkBoard.IsDifferent(oldBoard, SIZE, SIZE))
            {

                if (_WorkBoard.Any(SIZE, SIZE, x => x == 0))
                    InsertRandom();

                if (CheckWin())
                    return GameResult.Win;
                else if (CheckLost())
                    return GameResult.Loss;
                else
                    return GameResult.Continue;
            }
            else
                return GameResult.Continue;
        }

        private bool CheckLost()
        {
            return _WorkBoard.IsLost(SIZE);
        }

        private bool CheckWin()
        {
            return _WorkBoard.Any(SIZE, SIZE, x => x == WIN);
        }

        private void GoDirection(Direction direction)
        {
            bool revertXY = false, revertOrder = false;

            // go top
            switch (direction)
            {
                case Direction.Top:
                    revertXY = true;
                    break;
                case Direction.Bottom:
                    revertXY = true;
                    revertOrder = true;
                    break;
                case Direction.Left:
                    break;
                case Direction.Right:
                    revertOrder = true;
                    break;
            }
            bool[,] changed = new bool[SIZE, SIZE];
            changed.Fill(SIZE, SIZE, false);

            for (int x = 0; x < SIZE; x++)
            {
                bool changes;
                do
                {
                    changes = false;
                    if (DirectionChooser(changed, x, revertXY, revertOrder))
                        changes = true;
                }
                while (changes);
            }

        }

        public int[,] TryDirection(Direction direction)
        {
            int[,] board = new int[SIZE, SIZE];
            Array.Copy(_WorkBoard, 0, board, 0, SIZE * SIZE);
            Game2048 game = new Game2048(board);
            game.GoDirection(direction);
            return game.WorkBoard;
        }
        
        private bool DirectionChooser(bool[,] changed, int x, bool revertXY, bool revertOrder)
        {
            bool changes;
            changes = false;

            if (revertOrder)
            {
                for (int y = SIZE - 2; y >= 0; y--)
                {
                    if (InsideTheGo(changed, x, y, revertOrder, revertXY))
                        changes = true;
                }
            }
            else
            {
                for (int y = 1; y < SIZE; y++)
                {
                    if (InsideTheGo(changed, x, y, revertOrder, revertXY))
                        changes = true;
                }
            }
            return changes;
        }

        private bool InsideTheGo(bool[,] changed, int x, int y, bool revertOrder, bool revertXY)
        {
            int addremove = (revertOrder ? 1 : -1);

            int finalStartX = revertXY ? y : x;
            int finalStartY = revertXY ? x : y;
            int finalNextX = revertXY ? (finalStartX + addremove) : finalStartX;
            int finalNextY = revertXY ? finalStartY : (finalStartY + addremove);

            int me = _WorkBoard[finalStartX, finalStartY];
            if (me == 0)
                return false;

            int next = _WorkBoard[finalNextX, finalNextY];


            if ((next != 0) && (next != me || changed[finalNextX, finalNextY] || changed[finalStartX, finalStartY]))
                return false;


            _WorkBoard[finalNextX, finalNextY] = next == 0 ? me : (me + 1);
            _WorkBoard[finalStartX, finalStartY] = 0;
            if (next == 0)
                changed[finalNextX, finalNextY] = changed[finalStartX, finalStartY];
            else
                changed[finalNextX, finalNextY] = true;
            changed[finalStartX, finalStartY] = false;

            return true;
        }

    }
}
