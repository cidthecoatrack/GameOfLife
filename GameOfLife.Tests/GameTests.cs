using System;
using System.Linq;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class GameTests
    {
        private const Int32 MAX_TEST_SIZE = 100;

        [Test]
        public void RandomGridSize()
        {
            var game = new Game();
            Assert.That(game.Grid.EdgeSize, Is.AtLeast(1));
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void CannotHaveNegativeEdgeSize()
        {
            var game = new Game(-1);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void CannotHaveZeroEdgeSize()
        {
            var game = new Game(0);
        }

        [Test]
        public void SetGridSize()
        {
            var game = new Game(4);
            Assert.That(game.Grid.EdgeSize, Is.EqualTo(4));
        }

        [Test]
        public void SetAllCells()
        {
            var game = new Game(MAX_TEST_SIZE);
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

            game.SetLivingValues(aliveValues);

            for (var x = 0; x < MAX_TEST_SIZE; x++)
                for (var y = 0; y < MAX_TEST_SIZE; y++)
                    Assert.That(game.Grid.Cells[x][y].All(c => c == true), Is.True);
        }

        [Test]
        public void SetAllCellsDoesNotIncrementGeneration()
        {
            var game = new Game(MAX_TEST_SIZE);
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

            game.SetLivingValues(aliveValues);

            Assert.That(game.Generation, Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfNotCube()
        {
            var game = new Game(4);
            var aliveValues = new Boolean[4][][];

            for (var x = 0; x < 4; x++)
            {
                aliveValues[x] = new Boolean[4][];
                aliveValues[x][0] = new Boolean[4];
                aliveValues[x][1] = new Boolean[3];
                aliveValues[x][2] = new Boolean[4];
                aliveValues[x][3] = new Boolean[4];
            }

            game.SetLivingValues(aliveValues);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfWrongXSize()
        {
            var game = new Game(4);
            var aliveValues = new Boolean[3][][];

            for (var x = 0; x < 3; x++)
            {
                aliveValues[x] = new Boolean[4][];
                for(var y = 0; y < 4; y++)
                    aliveValues[x][y] = new Boolean[4];
            }

            game.SetLivingValues(aliveValues);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfWrongYSize()
        {
            var game = new Game(4);
            var aliveValues = new Boolean[4][][];

            for (var x = 0; x < 4; x++)
            {
                aliveValues[x] = new Boolean[3][];
                for (var y = 0; y < 3; y++)
                    aliveValues[x][y] = new Boolean[4];
            }

            game.SetLivingValues(aliveValues);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetAllCellsIfWrongZSize()
        {
            var game = new Game(4);
            var aliveValues = new Boolean[4][][];

            for (var x = 0; x < 4; x++)
            {
                aliveValues[x] = new Boolean[4][];
                for (var y = 0; y < 4; y++)
                    aliveValues[x][y] = new Boolean[3];
            }

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
            var game = new Game(1);
            game.Tick();

            Assert.That(game.Generation, Is.EqualTo(2));
        }

        [Test]
        public void AnyLiveCellWith00LiveNeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 0);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith01LiveNeighborDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 1);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith02LiveNeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 2);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith03LiveNeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 3);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith04NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 4);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith05NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 5);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith06NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 6);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyLiveCellWith07NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 7);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyLiveCellWith08NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 8);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyLiveCellWith09NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 9);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyLiveCellWith10NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 10);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyLiveCellWith11NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 11);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith12NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 12);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith13NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 13);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith14NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 14);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith15NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 15);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith16NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 16);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith17NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 17);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith18NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 18);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith19NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 19);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith20NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 20);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith21NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 21);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith22NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 22);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith23NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 23);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith24NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 24);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith25NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 25);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyLiveCellWith26NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(true, 26);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith00LiveNeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 0);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith01LiveNeighborDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 1);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith02LiveNeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 2);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith03LiveNeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 3);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith04NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 4);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith05NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 5);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith06NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 6);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith07NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 7);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith08NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 8);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith09NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 9);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyDeadCellWith10NeighborsLives()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 10);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void AnyDeadCellWith11NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 11);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith12NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 12);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith13NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 13);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith14NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 14);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith15NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 15);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith16NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 16);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith17NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 17);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith18NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 18);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith19NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 19);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith20NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 20);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith21NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 21);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith22NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 22);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith23NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 23);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith24NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 24);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith25NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 25);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void AnyDeadCellWith26NeighborsDies()
        {
            var game = new Game(3);
            var aliveValues = MakeNeighbors(false, 26);

            game.SetLivingValues(aliveValues);
            game.Tick();

            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        private Boolean[][][] MakeNeighbors(Boolean coreIsAlive, Int32 numberOfLivingNeighbors)
        {
            var aliveValues = new Boolean[3][][];
            for (var x = 0; x < 3; x++)
            {
                aliveValues[x] = new Boolean[3][];
                for (var y = 0; y < 3; y++)
                    aliveValues[x][y] = new Boolean[3];
            }

            aliveValues[1][1][1] = coreIsAlive;

            var nX = 0;
            var nY = 0;
            var nZ = 0;
            while (numberOfLivingNeighbors > 0 && nX < 3)
            {
                if (nX != 1 || nY != 1 || nZ != 1)
                {
                    aliveValues[nX][nY][nZ] = true;
                    numberOfLivingNeighbors--;
                }

                nZ++;
                if (nZ > 2)
                {
                    nZ = 0;
                    nY++;
                    if (nY > 2)
                    {
                        nY = 0;
                        nX++;
                    }
                }
            }

            return aliveValues;
        }

        [Test]
        public void NotNeighbors()
        {
            var game = new Game(5);

            for (var x1 = 0; x1 < 5; x1++)
            {
                for (var y1 = 0; y1 < 5; y1++)
                {
                    for (var z1 = 0; z1 < 5; z1++)
                    {
                        if (x1 == 2 && y1 == 2 && z1 == 2)
                            continue;

                        if (x1 != 0 && x1 != 4 && y1 != 0 && y1 != 4 && z1 != 0 && z1 != 4)
                            continue;

                        for (var x2 = x1; x2 < 5; x2++)
                        {
                            for (var y2 = y1; y2 < 5; y2++)
                            {
                                for (var z2 = z1; z2 < 3; z2++)
                                {
                                    if (x2 == 2 && y2 == 2 && z2 == 2)
                                        continue;

                                    if (x2 != 0 && x2 != 4 && y2 != 0 && y2 != 4 && z2 != 0 && z2 != 4)
                                        continue;

                                    if (x2 == x1 && y2 == y1 && z2 == z1)
                                        continue;

                                    var aliveValues = new Boolean[5][][];
                                    for (var x = 0; x < 5; x++)
                                    {
                                        aliveValues[x] = new Boolean[5][];
                                        for (var y = 0; y < 5; y++)
                                            aliveValues[x][y] = new Boolean[5];
                                    }

                                    aliveValues[1][1][1] = true;
                                    aliveValues[x1][y1][z1] = true;
                                    aliveValues[x2][y2][z2] = true;

                                    game.SetLivingValues(aliveValues);
                                    game.Tick();

                                    var failMessage = String.Format("p1: ({0},{1})\np2: ({2},{3})", x1, y1, x2, y2);
                                    Assert.That(game.Grid.Cells[2][2][2], Is.False, failMessage);
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void Neighbors()
        {
            var game = new Game(3);

            for (var x1 = 0; x1 < 3; x1++)
            {
                for (var y1 = 0; y1 < 3; y1++)
                {
                    for (var z1 = 0; z1 < 3; z1++)
                    {
                        if (x1 == 1 && y1 == 1 && z1 == 1)
                            continue;

                        var aliveValues = MakeNeighbors(true, 5);

                        if (aliveValues[x1][y1][z1])
                            continue;

                        aliveValues[x1][y1][z1] = true;

                        game.SetLivingValues(aliveValues);
                        game.Tick();

                        var failMessage = String.Format("point: ({0},{1})", x1, y1);
                        Assert.That(game.Grid.Cells[1][1][1], Is.True, failMessage);
                    }
                }
            }
        }
    }
}