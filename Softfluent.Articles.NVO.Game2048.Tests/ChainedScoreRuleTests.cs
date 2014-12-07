using Microsoft.VisualStudio.TestTools.UnitTesting;
using Softfluent.Articles.NVO.Game2048.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softfluent.Articles.NVO.Game2048.Tests
{  
    /// <summary>
    ///This is a test class for SmoothnessRuleTest and is intended
    ///to contain all SmoothnessRuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ChainedScoreRuleTests
    {

        
        /// <summary>
        ///A test for CalculatePoints
        ///</summary>
        [TestMethod()]
        public void CalculateChainedPointsTest1()
        {
            ChainedScoreRule target = new ChainedScoreRule(1) { CurrentEvolutionState = 1 };
            var grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 1, 2 }, 
                       { 0, 0, 0, 3 }, 
                       { 0, 0, 3, 4 } };
            var result = target.CalculatePoints(grid, Direction.Bottom, 4);
            
            Assert.AreEqual(result,3);

            grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 0, 0, 0, 0 }, 
                       { 0, 2, 4, 2 }, 
                       { 0, 0, 2, 1 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, 0);


            grid = new int[,] {
                       { 0, 0, 1, 0 }, 
                       { 0, 3, 3, 1 }, 
                       { 1, 3, 4, 0 }, 
                       { 0, 2, 0, 0 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, 2);

          grid = new int[,] {
                       { 0, 0, 0, 0 }, 
                       { 2, 1, 2, 2 }, 
                       { 4, 3, 2, 2 }, 
                       { 5, 6, 7, 8 } };
            result = target.CalculatePoints(grid, Direction.Bottom, 4);

            Assert.AreEqual(result, 6);
        }
    }
}
