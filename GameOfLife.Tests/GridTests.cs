using System;
using System.Linq;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GridTests
    {
        private const Int32 MAX_TEST_SIZE = 1000;

        [Test]
        public void Grid1x1()
        {
            var grid = new Grid(1, 1);

            Assert.That(grid.XSize, Is.EqualTo(1));
            Assert.That(grid.YSize, Is.EqualTo(1));
        }

        [Test]
        public void Grid4x4()
        {
            var grid = new Grid(4, 4);

            Assert.That(grid.XSize, Is.EqualTo(4));
            Assert.That(grid.YSize, Is.EqualTo(4));
        }

        [Test]
        public void IndependentXAndY()
        {
            var grid = new Grid(2, 3);

            Assert.That(grid.XSize, Is.EqualTo(2));
            Assert.That(grid.YSize, Is.EqualTo(3));
        }

        [Test]
        public void VariableXAndY()
        {
            var randomizer = new Random();
            var x = randomizer.Next(1, 1001);
            var y = randomizer.Next(1, 1001);

            var grid = new Grid(x, y);

            Assert.That(grid.XSize, Is.EqualTo(x));
            Assert.That(grid.YSize, Is.EqualTo(y));
        }

        [Test]
        public void CellsRandomizedAtStart()
        {
            var grid = new Grid(MAX_TEST_SIZE, MAX_TEST_SIZE);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
            {
                Assert.That(grid.Cells[x].Any(c => c.Alive), Is.True);
                Assert.That(grid.Cells[x].Any(c => !c.Alive), Is.True);
            }
        }

        [Test]
        public void TickSetsAllCellsInNextGeneration()
        {
            var grid = new Grid(MAX_TEST_SIZE, MAX_TEST_SIZE);

            for(var x = 0; x < 4; x++)
                for(var y = 0; y < 4; y++)
                    grid.Cells[x][y].AliveNextGeneration = false;

            grid.Tick();

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                Assert.That(grid.Cells[x].All(c => !c.Alive), Is.True);
        }

        [Test]
        public void SetAllCells()
        {
            var grid = new Grid(MAX_TEST_SIZE, MAX_TEST_SIZE);
            var aliveValues = new Boolean[MAX_TEST_SIZE][];

            for (var x = 0; x < aliveValues.Length; x++)
            {
                aliveValues[x] = new Boolean[MAX_TEST_SIZE];

                for (var y = 0; y < aliveValues[x].Length; y++)
                    aliveValues[x][y] = true;
            }

            grid.SetLivingValues(aliveValues);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                Assert.That(grid.Cells[x].All(c => c.Alive), Is.True);
        }
    }
}