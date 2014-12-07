using Softfluent.Articles.NVO.Game2048.AI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Softfluent.Articles.NVO.Game2048;

namespace Softfluent.Articles.NVO.Game2048.Tests
{
    
    
    /// <summary>
    ///This is a test class for SmoothnessRuleTest and is intended
    ///to contain all SmoothnessRuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SmoothnessRuleTest
    {
        
        /// <summary>
        ///A test for CalculatePoints
        ///</summary>
        [TestMethod()]
        public void CalculateSmoothPointsTest1()
        {
            SmoothnessRule target = new SmoothnessRule(1);
            var grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 3 }, 
                       { 0, 0, 3, 4 } };
            var result = target.CalculatePoints(grid, Direction.Bottom, 4);
            
            Assert.AreEqual(result,-2);

            grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 4, 3 }, 
                       { 0, 0, 3, 1 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, -6);


            grid = new int[,] {
                       { 0, 0, 1, 0 }, 
                       { 0, 3, 3, 1 }, 
                       { 1, 3, 4, 0 }, 
                       { 0, 0, 0, 0 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, -8);
        }
    }
}
