using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Softfluent.Articles.NVO.Game2048.Tests
{
    [TestClass]
    public class DirectionTests
    {
        [TestMethod]
        public void TestRight()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 1, 1, 0, 0 }, 
                       { 1, 1, 2, 3 }, 
                       { 1, 2, 1, 2 }, 
                       { 2, 2, 1, 1 }});
            
            
            int[,] expected = new int[,] {
                       { 0, 0, 0, 2 }, 
                       { 0, 2, 2, 3 }, 
                       { 1, 2, 1, 2 }, 
                       { 0, 0, 3, 2 } };

            var result = game.TryDirection(Direction.Right);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }
        [TestMethod]
        public void TestRight2()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 0, 1, 1, 1 }, 
                       { 1, 1, 1, 0 }, 
                       { 1, 1, 0, 1 }, 
                       { 1, 0, 1, 1 }});


            int[,] expected = new int[,] {
                       { 0, 0, 1, 2 }, 
                       { 0, 0, 1, 2 }, 
                       { 0, 0, 1, 2 }, 
                       { 0, 0, 1, 2 } };

            var result = game.TryDirection(Direction.Right);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }


        [TestMethod]
        public void TestLeft()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 0, 0, 1, 1 }, 
                       { 3, 2, 1, 1 }, 
                       { 2, 1, 2, 1 }, 
                       { 1, 1, 2, 2 }});
        
            int[,] expected = new int[,] {
                       { 2, 0, 0, 0 }, 
                       { 3, 2, 2, 0 }, 
                       { 2, 1, 2, 1 }, 
                       { 2, 3, 0, 0 } };

            var result = game.TryDirection(Direction.Left);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }

        [TestMethod]
        public void TestLeft2()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 0, 1, 1, 1 }, 
                       { 1, 1, 1, 0 }, 
                       { 1, 1, 0, 1 }, 
                       { 1, 0, 1, 1 }});

            int[,] expected = new int[,] {
                       { 2, 1, 0, 0 }, 
                       { 2, 1, 0, 0 }, 
                       { 2, 1, 0, 0 }, 
                       { 2, 1, 0, 0 } };

            var result = game.TryDirection(Direction.Left);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }
        [TestMethod]
        public void TestTop()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 0, 3, 2, 1 }, 
                       { 0, 2, 1, 1 }, 
                       { 1, 1, 2, 2 }, 
                       { 1, 1, 1, 2 }});

            int[,] expected = new int[,] {
                       { 2, 3, 2, 2 }, 
                       { 0, 2, 1, 3 }, 
                       { 0, 2, 2, 0 }, 
                       { 0, 0, 1, 0 }};

            var result = game.TryDirection(Direction.Top);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }
        [TestMethod]
        public void TestTop2()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 0, 1, 1, 1 }, 
                       { 1, 1, 1, 0 }, 
                       { 1, 1, 0, 1 }, 
                       { 1, 0, 1, 1 }});

            int[,] expected = new int[,] {
                       { 2, 2, 2, 2 }, 
                       { 1, 1, 1, 1 }, 
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }};

            var result = game.TryDirection(Direction.Top);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }
        [TestMethod]
        public void TestBottom()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 1, 1, 1, 2 }, 
                       { 1, 1, 2, 2 }, 
                       { 0, 2, 1, 1 }, 
                       { 0, 3, 2, 1 }});

            int[,] expected = new int[,] {
                       { 0, 0, 1, 0 }, 
                       { 0, 2, 2, 0 }, 
                       { 0, 2, 1, 3 }, 
                       { 2, 3, 2, 2 }};

            var result = game.TryDirection(Direction.Bottom);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }

        [TestMethod]
        public void TestBottom2()
        {
            Game2048 game = new Game2048(new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 1, 1, 1, 1 }, 
                       { 1, 1, 1, 1 }, 
                       { 1, 1, 1, 1 }});

            int[,] expected = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }, 
                       { 1, 1, 1, 1 }, 
                       { 2, 2, 2, 2 }};

            var result = game.TryDirection(Direction.Bottom);

            expected.AssertBoardEquals(result, Game2048.SIZE);
        }
    }
}
