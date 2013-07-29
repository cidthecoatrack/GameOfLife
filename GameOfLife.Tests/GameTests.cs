using System;
using System.Collections.Generic;
using System.Linq;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void RandomGridSize()
        {
            var game = new Game();
            Assert.That(game.Grid.XSize, Is.AtLeast(1));
            Assert.That(game.Grid.YSize, Is.AtLeast(1));
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void CannotHaveNegativeX()
        {
            var game = new Game(-1, 1);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void CannotHaveNegativeY()
        {
            var game = new Game(1, -1);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void CannotHaveZeroX()
        {
            var game = new Game(0, 1);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void CannotHaveZeroY()
        {
            var game = new Game(1, 0);
        }

        [Test]
        public void SetGridSize()
        {
            var game = new Game(4, 3);
            Assert.That(game.Grid.XSize, Is.EqualTo(4));
            Assert.That(game.Grid.YSize, Is.EqualTo(3));
        }

        [Test]
        public void SetAllCells()
        {
            var game = new Game(4, 4);
            var aliveValues = new List<Boolean>();

            for (var i = 0; i < game.Grid.Cells.Count; i++)
                aliveValues.Add(true);

            game.SetLivingValues(aliveValues);

            Assert.That(game.Grid.Cells.All(c => c.Alive), Is.True);
        }

        [Test]
        public void SetAllCellsDoesNotIncrementGeneration()
        {
            var game = new Game(4, 4);
            var aliveValues = new List<Boolean>();

            for (var i = 0; i < game.Grid.Cells.Count; i++)
                aliveValues.Add(true);

            game.SetLivingValues(aliveValues);

            Assert.That(game.Generation, Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfWrongNumberOfValues()
        {
            var game = new Game(4, 4);
            var aliveValues = new List<Boolean>();
            aliveValues.Add(true);

            game.SetLivingValues(aliveValues);
        }

        [Test]
        public void Generation()
        {
            var game = new Game();
            Assert.That(game.Generation, Is.EqualTo(1));
        }

        [Test]
        public void TickIncreasesGeneration()
        {
            var game = new Game(1, 1);
            game.Tick();

            Assert.That(game.Generation, Is.EqualTo(2));
        }

        [Test]
        public void AnyLiveCellWith0LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {false, false, false,
                                                   false, true, false,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells.Any(c => c.Alive), Is.False);
        }

        [Test]
        public void AnyLiveCellWith1LiveNeighborDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {false, false, false,
                                                   true,  true,  false,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells.Any(c => c.Alive), Is.False);
        }

        [Test]
        public void AnyLiveCellWith2LiveNeighborsLives()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  false, false,
                                                   true,  true,  false,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.True);
        }

        [Test]
        public void AnyLiveCellWith3LiveNeighborsLives()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  false, false,
                                                   true,  true,  true,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.True);
        }

        [Test]
        public void AnyLiveCellWith4NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  false,
                                                   true,  true,  true,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith5NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  true,  true,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith6NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  true,  true,
                                                   true,  false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith7NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  true,  true,
                                                   true,  false, true};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith8NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  true,  true,
                                                   true,  true,  true};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith0LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {false, false, false,
                                                   false, false, false,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith1LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  false, false,
                                                   false, false, false,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith2LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  false, false,
                                                   true,  false, false,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith3LiveNeighborsLives()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  false, false,
                                                   true,  false, true,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.True);
        }

        [Test]
        public void AnyDeadCellWith4LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  false,
                                                   true,  false, true,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith5LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  false, true,
                                                   false, false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith6LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  false, true,
                                                   true,  false, false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith7LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  false, true,
                                                   true,  true,  false};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith8LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new List<Boolean>() {true,  true,  true,
                                                   true,  false, true,
                                                   true,  true,  true};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void NotNeighbors()
        {
            var game = new Game(5, 5);
            var aliveValues = new List<Boolean>() {true,  true,  true,  true,  true,
                                                   true,  false, false, false, true,
                                                   true,  false, true,  false, true,
                                                   true,  false, false, false, true,
                                                   true,  true,  true,  true,  true};

            game.SetLivingValues(aliveValues);
            game.Tick();

            var cell = game.Grid.Cells.First(c => c.X == 2 && c.Y == 2);

            Assert.That(cell.Alive, Is.False);
        }

        [Test]
        public void Neighbors()
        {
            var game = new Game(3, 3);

            for (var i = 0; i < 8; i++)
            {
                if (i == 4)
                    continue;

                for (var j = i + 1; j < 9; j++)
                {
                    if (j == 4)
                        continue;

                    var aliveValues = new List<Boolean>() {false, false, false,
                                                           false, true,  false,
                                                           false, false, false};

                    aliveValues[i] = true;
                    aliveValues[j] = true;

                    game.SetLivingValues(aliveValues);
                    game.Tick();

                    var cell = game.Grid.Cells.First(c => c.X == 1 && c.Y == 1);

                    Assert.That(cell.Alive, Is.True);
                }
            }
        }
    }
}