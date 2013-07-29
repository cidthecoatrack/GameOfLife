using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class CellTests
    {
        [Test]
        public void SetCoordinates()
        {
            var cell = new Cell(2, 3, true);

            Assert.That(cell.X, Is.EqualTo(2));
            Assert.That(cell.Y, Is.EqualTo(3));
        }

        [Test]
        public void AliveCell()
        {
            var cell = new Cell(1, 1, true);
            Assert.That(cell.Alive, Is.True);
        }

        [Test]
        public void DeadCell()
        {
            var cell = new Cell(1, 1, false);
            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void CanSetNextGeneration()
        {
            var cell = new Cell(1, 1, true);
            cell.AliveNextGeneration = false;

            Assert.That(cell.AliveNextGeneration, Is.False);
        }

        [Test]
        public void TickSetsAliveFromNextGeneration()
        {
            var cell = new Cell(1, 1, true);
            cell.AliveNextGeneration = false;
            cell.Tick();

            Assert.That(cell.Alive, Is.False);
        }
    }
}