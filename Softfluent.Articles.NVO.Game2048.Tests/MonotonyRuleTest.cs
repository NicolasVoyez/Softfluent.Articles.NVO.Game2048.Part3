using Softfluent.Articles.NVO.Game2048.AI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Softfluent.Articles.NVO.Game2048;

namespace Softfluent.Articles.NVO.Game2048.Tests
{
    
    
    /// <summary>
    ///This is a test class for MonotonyRuleTest and is intended
    ///to contain all MonotonyRuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MonotonyRuleTest
    {
        /// <summary>
        ///A test for CalculatePoints
        ///</summary>
        [TestMethod()]
        public void CalculateMonotonyPointsTest1()
        {
            MonotonyRule target = new MonotonyRule(1);
            var grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 3 }, 
                       { 0, 0, 3, 4 } };
            var result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, -2);

            grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 4, 3 }, 
                       { 0, 0, 1, 3 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, -5);


            grid = new int[,] {
                       { 0, 0, 1, 0 }, 
                       { 0, 3, 3, 1 }, 
                       { 1, 3, 4, 0 }, 
                       { 0, 0, 0, 0 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

           Assert.AreEqual(result, -16);
        }
        
    }
}
