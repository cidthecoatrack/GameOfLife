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
        public void DefaultGridSize()
        {
            var game = new Game();
            Assert.That(game.Grid.EdgeSize, Is.EqualTo(15));
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
            var game = new Game();
            game.Tick();

            Assert.That(game.Generation, Is.EqualTo(2));
        }

        [Test]
        public void SetMinimumLiveValue()
        {
            var game = new Game(3);
            game.MinLive = 1;

            var aliveValues = MakeNeighbors(true, 0);
            game.SetLivingValues(aliveValues);

            game.Tick();
            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void SetMaximumLiveValue()
        {
            var game = new Game(3);
            game.MaxLive = 7;

            var aliveValues = MakeNeighbors(true, 8);
            game.SetLivingValues(aliveValues);

            game.Tick();
            Assert.That(game.Grid.Cells[1][1][1], Is.False);
        }

        [Test]
        public void SetMinimumBirthValue()
        {
            var game = new Game(3);
            game.MinBorn = 0;

            var aliveValues = MakeNeighbors(false, 0);
            game.SetLivingValues(aliveValues);

            game.Tick();
            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void SetMaximumBirthValue()
        {
            var game = new Game(3);
            game.MaxBorn = 8;

            var aliveValues = MakeNeighbors(false, 8);
            game.SetLivingValues(aliveValues);

            game.Tick();
            Assert.That(game.Grid.Cells[1][1][1], Is.True);
        }

        [Test]
        public void Live()
        {
            var game = new Game(3);

            for (game.MinLive = 0; game.MinLive < 9; game.MinLive++)
            {
                for (game.MaxLive = game.MinLive; game.MaxLive < 9; game.MaxLive++)
                {
                    var neighbors = game.MinLive;
                    while (neighbors <= game.MaxLive)
                    {
                        var aliveValues = MakeNeighbors(true, neighbors);
                        game.SetLivingValues(aliveValues);

                        game.Tick();
                        Assert.That(game.Grid.Cells[1][1][1], Is.True);

                        neighbors++;
                    }
                }
            }
        }

        [Test]
        public void Die()
        {
            var game = new Game(3);

            for (game.MinLive = 0; game.MinLive < 9; game.MinLive++)
            {
                for (game.MaxLive = game.MinLive; game.MaxLive < 9; game.MaxLive++)
                {
                    var neighbors = 0;
                    while (neighbors < game.MinLive)
                    {
                        var aliveValues = MakeNeighbors(true, neighbors);
                        game.SetLivingValues(aliveValues);

                        game.Tick();
                        Assert.That(game.Grid.Cells[1][1][1], Is.False);

                        neighbors++;
                    }

                    neighbors = game.MaxLive + 1;
                    while (neighbors < 9)
                    {
                        var aliveValues = MakeNeighbors(true, neighbors);
                        game.SetLivingValues(aliveValues);

                        game.Tick();
                        Assert.That(game.Grid.Cells[1][1][1], Is.False);

                        neighbors++;
                    }
                }
            }
        }

        [Test]
        public void Born()
        {
            var game = new Game(3);

            for (game.MinBorn = 0; game.MinBorn < 9; game.MinBorn++)
            {
                for (game.MaxBorn = game.MinBorn; game.MaxBorn < 9; game.MaxBorn++)
                {
                    var neighbors = game.MinBorn;
                    while (neighbors <= game.MaxBorn)
                    {
                        var aliveValues = MakeNeighbors(false, neighbors);
                        game.SetLivingValues(aliveValues);

                        game.Tick();
                        Assert.That(game.Grid.Cells[1][1][1], Is.True);

                        neighbors++;
                    }
                }
            }
        }

        [Test]
        public void StayDead()
        {
            var game = new Game(3);

            for (game.MinBorn = 0; game.MinBorn < 9; game.MinBorn++)
            {
                for (game.MaxBorn = game.MinBorn; game.MaxBorn < 9; game.MaxBorn++)
                {
                    var neighbors = 0;
                    while (neighbors < game.MinBorn)
                    {
                        var aliveValues = MakeNeighbors(false, neighbors);
                        game.SetLivingValues(aliveValues);

                        game.Tick();
                        Assert.That(game.Grid.Cells[1][1][1], Is.False);

                        neighbors++;
                    }

                    neighbors = game.MaxBorn + 1;
                    while (neighbors < 9)
                    {
                        var aliveValues = MakeNeighbors(false, neighbors);
                        game.SetLivingValues(aliveValues);

                        game.Tick();
                        Assert.That(game.Grid.Cells[1][1][1], Is.False);

                        neighbors++;
                    }
                }
            }
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
        public void Running()
        {
            var game = new Game(1);

            var aliveValues = new Boolean[1][][];
            aliveValues[0] = new Boolean[1][];
            aliveValues[0][0] = new Boolean[1] { true };
            game.SetLivingValues(aliveValues);

            game.Tick();
            Assert.That(game.NotRunning, Is.False);

            game.Tick();
            Assert.That(game.NotRunning, Is.True);
        }

        [Test]
        public void OnceNotRunningNeverStartsRunningAgain()
        {
            var game = new Game(1);

            var aliveValues = new Boolean[1][][];
            aliveValues[0] = new Boolean[1][];
            aliveValues[0][0] = new Boolean[1] { false };
            game.SetLivingValues(aliveValues);

            var count = 0;
            while (count < 1000000)
            {
                game.Tick();
                Assert.That(game.NotRunning, Is.True);
                count++;
            }
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

                        var aliveValues = MakeNeighbors(true, 1);

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