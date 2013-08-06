using System;
using System.Linq;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GridTests
    {
        private const Int32 MAX_TEST_SIZE = 100;

        [Test]
        public void Grid1x1()
        {
            var grid = new Grid(1);

            Assert.That(grid.EdgeSize, Is.EqualTo(1));
        }

        [Test]
        public void Grid4x4()
        {
            var grid = new Grid(4);

            Assert.That(grid.EdgeSize, Is.EqualTo(4));
        }

        [Test]
        public void CellsRandomizedAtStart()
        {
            var grid = new Grid(MAX_TEST_SIZE);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
            {
                for (var y = 0; y < MAX_TEST_SIZE; y++)
                {
                    Assert.That(grid.Cells[x][y].Any(c => c == true), Is.True);
                    Assert.That(grid.Cells[x][y].Any(c => c == false), Is.True);
                }
            }
        }

        [Test]
        public void TickSetsAllCellsInNextGeneration()
        {
            var grid = new Grid(MAX_TEST_SIZE);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                for (var y = 0; y < MAX_TEST_SIZE; y++)
                    for (var z = 0; z < MAX_TEST_SIZE; z++)
                        grid.NextGeneration[x][y][z] = false;

            grid.Tick();

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                for (var y = 0; y < MAX_TEST_SIZE; y++)
                    Assert.That(grid.Cells[x][y].All(c => c == false), Is.True);
        }

        [Test]
        public void SetAllCells()
        {
            var grid = new Grid(MAX_TEST_SIZE);
            var aliveValues = new Boolean[MAX_TEST_SIZE][][];

            for (var x = 0; x < MAX_TEST_SIZE; x++)
            {
                aliveValues[x] = new Boolean[MAX_TEST_SIZE][];
                for (var y = 0; y < MAX_TEST_SIZE; y++)
                {
                    aliveValues[x][y] = new Boolean[MAX_TEST_SIZE];
                    for (var z = 0; z < MAX_TEST_SIZE; z++)
                        aliveValues[x][y][z] = true;
                }
            }

            grid.SetLivingValues(aliveValues);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                for (var y = 0; y < MAX_TEST_SIZE; y++)
                    Assert.That(grid.Cells[x][y].All(c => c == true), Is.True);
        }
    }
}