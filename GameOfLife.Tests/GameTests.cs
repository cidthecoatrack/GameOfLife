using System;
using System.Linq;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GameTests
    {
        private const Int32 MAX_TEST_SIZE = 1000;
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
            var game = new Game(MAX_TEST_SIZE, MAX_TEST_SIZE);
            var aliveValues = new Boolean[MAX_TEST_SIZE][];

            for (var x = 0; x < aliveValues.Length; x++)
            {
                aliveValues[x] = new Boolean[MAX_TEST_SIZE];
                for (var y = 0; y < aliveValues[x].Length; y++)
                    aliveValues[x][y] = true;
            }

            game.SetLivingValues(aliveValues);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                Assert.That(game.Grid.Cells[x].All(c => c.Alive), Is.True);
        }

        [Test]
        public void SetAllCellsDoesNotIncrementGeneration()
        {
            var game = new Game(MAX_TEST_SIZE, MAX_TEST_SIZE);
            var aliveValues = new Boolean[MAX_TEST_SIZE][];

            for (var x = 0; x < aliveValues.Length; x++)
            {
                aliveValues[x] = new Boolean[MAX_TEST_SIZE];
                for (var y = 0; y < aliveValues[x].Length; y++)
                    aliveValues[x][y] = true;
            }

            game.SetLivingValues(aliveValues);

            Assert.That(game.Generation, Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfNotRectangularGrid()
        {
            var game = new Game(4, 4);
            var aliveValues = new Boolean[4][];
            aliveValues[0] = new Boolean[4];
            aliveValues[1] = new Boolean[3];
            aliveValues[2] = new Boolean[4];
            aliveValues[3] = new Boolean[4];

            game.SetLivingValues(aliveValues);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfWrongXSize()
        {
            var game = new Game(4, 4);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[4];
            aliveValues[1] = new Boolean[4];
            aliveValues[2] = new Boolean[4];

            game.SetLivingValues(aliveValues);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfWrongYSize()
        {
            var game = new Game(4, 4);
            var aliveValues = new Boolean[4][];
            aliveValues[0] = new Boolean[3];
            aliveValues[1] = new Boolean[3];
            aliveValues[2] = new Boolean[3];
            aliveValues[3] = new Boolean[3];

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
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, false };
            aliveValues[1] = new Boolean[3] { false, true,  false };
            aliveValues[2] = new Boolean[3] { false, false, false };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith1LiveNeighborDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true  };
            aliveValues[1] = new Boolean[3] { false, true,  false };
            aliveValues[2] = new Boolean[3] { false, false, false };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith2LiveNeighborsLives()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true  };
            aliveValues[1] = new Boolean[3] { false, true,  true  };
            aliveValues[2] = new Boolean[3] { false, false, false };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.True);
        }

        [Test]
        public void AnyLiveCellWith3LiveNeighborsLives()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, true,  true };
            aliveValues[2] = new Boolean[3] { false, false, true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.True);
        }

        [Test]
        public void AnyLiveCellWith4NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, true,  true };
            aliveValues[2] = new Boolean[3] { false, true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith5NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, true,  true };
            aliveValues[2] = new Boolean[3] { true,  true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith6NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { true,  true,  true };
            aliveValues[2] = new Boolean[3] { true,  true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith7NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { true, false, true };
            aliveValues[1] = new Boolean[3] { true, true,  true };
            aliveValues[2] = new Boolean[3] { true, true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyLiveCellWith8NeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { true, true, true };
            aliveValues[1] = new Boolean[3] { true, true, true };
            aliveValues[2] = new Boolean[3] { true, true, true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith0LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, false };
            aliveValues[1] = new Boolean[3] { false, false, false };
            aliveValues[2] = new Boolean[3] { false, false, false };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith1LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, false, false };
            aliveValues[2] = new Boolean[3] { false, false, false };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith2LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, false, false };
            aliveValues[2] = new Boolean[3] { false, true,  false };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith3LiveNeighborsLives()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, false, true };
            aliveValues[2] = new Boolean[3] { false, false, true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.True);
        }

        [Test]
        public void AnyDeadCellWith4LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, false, true };
            aliveValues[2] = new Boolean[3] { false, true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith5LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { false, false, true };
            aliveValues[2] = new Boolean[3] { true,  true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith6LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { false, false, true };
            aliveValues[1] = new Boolean[3] { true,  false, true };
            aliveValues[2] = new Boolean[3] { true,  true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith7LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { true, false, true };
            aliveValues[1] = new Boolean[3] { true, false, true };
            aliveValues[2] = new Boolean[3] { true, true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void AnyDeadCellWith8LiveNeighborsDies()
        {
            var game = new Game(3, 3);
            var aliveValues = new Boolean[3][];
            aliveValues[0] = new Boolean[3] { true, true,  true };
            aliveValues[1] = new Boolean[3] { true, false, true };
            aliveValues[2] = new Boolean[3] { true, true,  true };

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1].Alive, Is.False);
        }

        [Test]
        public void NotNeighbors()
        {
            var game = new Game(5, 5);

            for (var x1 = 0; x1 < 5; x1++)
            {
                for (var y1 = 0; y1 < 5; y1++)
                {
                    if (y1 == 2 && x1 == 2)
                        continue;

                    if (x1 != 0 && x1 != 4 && y1 != 0 && y1 != 4)
                        continue;

                    for (var x2 = x1; x2 < 5; x2++)
                    {
                        for (var y2 = y1; y2 < 5; y2++)
                        {
                            if (y2 == 2 && x2 == 2)
                                continue;

                            if (x2 != 0 && x2 != 4 && y2 != 0 && y2 != 4)
                                continue;

                            if (y2 == y1 && x2 == x1)
                                continue;

                            var aliveValues = new Boolean[5][];
                            aliveValues[0] = new Boolean[5] { false, false, false, false, false };
                            aliveValues[1] = new Boolean[5] { false, false, false, false, false };
                            aliveValues[2] = new Boolean[5] { false, false, true,  false, false };
                            aliveValues[3] = new Boolean[5] { false, false, false, false, false };
                            aliveValues[4] = new Boolean[5] { false, false, false, false, false };

                            aliveValues[x1][y1] = true;
                            aliveValues[x2][y2] = true;

                            game.SetLivingValues(aliveValues);
                            game.Tick();

                            var failMessage = String.Format("p1: ({0},{1})\np2: ({2},{3})", x1, y1, x2, y2);
                            Assert.That(game.Grid.Cells[2][2].Alive, Is.False, failMessage);
                        }
                    }
                }
            }
        }

        [Test]
        public void Neighbors()
        {
            var game = new Game(3, 3);

            for (var x1 = 0; x1 < 3; x1++)
            {
                for (var y1 = 0; y1 < 3; y1++)
                {
                    if (y1 == 1 && x1 == 1)
                        continue;

                    for (var x2 = x1; x2 < 3; x2++)
                    {
                        for (var y2 = y1; y2 < 3; y2++)
                        {
                            if (y2 == 1 && x2 == 1)
                                continue;

                            if (y2 == y1 && x2 == x1)
                                continue;

                            var aliveValues = new Boolean[3][];
                            aliveValues[0] = new Boolean[3] { false, false, false };
                            aliveValues[1] = new Boolean[3] { false, true,  false };
                            aliveValues[2] = new Boolean[3] { false, false, false };

                            aliveValues[x1][y1] = true;
                            aliveValues[x2][y2] = true;

                            game.SetLivingValues(aliveValues);
                            game.Tick();

                            var failMessage = String.Format("p1: ({0},{1})\np2: ({2},{3})", x1, y1, x2, y2);
                            Assert.That(game.Grid.Cells[1][1].Alive, Is.True, failMessage);
                        }
                    }
                }
            }
        }

        [Test]
        public void SingleTickTimeOf1000x1000()
        {
            var game = new Game(1000, 1000);
            game.Execute(1);
        }
    }
}