using System;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void Grid1x1()
        {
            var grid = new Grid(1, 1);

            Assert.That(grid.XSize, Is.EqualTo(1));
            Assert.That(grid.YSize, Is.EqualTo(1));
            Assert.That(grid.Cells.Count, Is.EqualTo(1));
        }

        [Test]
        public void Grid4x4()
        {
            var grid = new Grid(4, 4);

            Assert.That(grid.XSize, Is.EqualTo(4));
            Assert.That(grid.YSize, Is.EqualTo(4));
            Assert.That(grid.Cells.Count, Is.EqualTo(16));
        }

        [Test]
        public void IndependentXAndY()
        {
            var grid = new Grid(2, 3);

            Assert.That(grid.XSize, Is.EqualTo(2));
            Assert.That(grid.YSize, Is.EqualTo(3));
            Assert.That(grid.Cells.Count, Is.EqualTo(6));
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
            Assert.That(grid.Cells.Count, Is.EqualTo(x * y));
        }

        [Test]
        public void CellsRandomizedAtStart()
        {
            var grid = new Grid(1000, 1000);

            Assert.That(grid.Cells.Any(c => c.Alive), Is.True);
            Assert.That(grid.Cells.Any(c => !c.Alive), Is.True);
        }

        [Test]
        public void TickSetsAllCellsInNextGeneration()
        {
            var grid = new Grid(4, 4);

            foreach (var cell in grid.Cells)
                cell.AliveNextGeneration = false;

            grid.Tick();

            Assert.That(grid.Cells.All(c => !c.Alive), Is.True);
        }

        [Test]
        public void SetAllCells()
        {
            var grid = new Grid(4, 4);
            var aliveValues = new List<Boolean>();

            for (var i = 0; i < grid.Cells.Count; i++)
                aliveValues.Add(true);

            grid.SetLivingValues(aliveValues);

            Assert.That(grid.Cells.All(c => c.Alive), Is.True);
        }
    }
}