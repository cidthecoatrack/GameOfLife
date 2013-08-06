using System;

namespace GameOfLife.Core
{
    public class Game
    {
        public Grid Grid { get; private set; }
        public Int32 Generation { get; private set; }

        public Game()
        {
            var randomizer = new Random();
            var edgeSize = randomizer.Next(1, 101);

            Grid = new Grid(edgeSize);
            Generation = 1;
        }

        public Game(Int32 edgeSize)
        {
            Grid = new Grid(edgeSize);
            Generation = 1;
        }

        public void Reset()
        {
            Grid = new Grid(Grid.EdgeSize);
            Generation = 1;
        }

        public void SetLivingValues(Boolean[][][] newValues)
        {
            if (Grid.EdgeSize != newValues.Length)
                throw new ArgumentOutOfRangeException();

            foreach (var cellLayer in newValues)
            {
                if (Grid.EdgeSize != cellLayer.Length)
                    throw new ArgumentOutOfRangeException();

                foreach (var cellRow in cellLayer)
                    if (Grid.EdgeSize != cellRow.Length)
                        throw new ArgumentOutOfRangeException();
            }

            Grid.SetLivingValues(newValues);
        }

        public void Tick()
        {
            for (var x = 0; x < Grid.EdgeSize; x++)
            {
                for (var y = 0; y < Grid.EdgeSize; y++)
                {
                    for (var z = 0; z < Grid.EdgeSize; z++)
                    {
                        var livingNeighbors = LivingNeighborsOf(x, y, z);

                        if (Grid.Cells[x][y][z])
                            Grid.NextGeneration[x][y][z] = livingNeighbors >= 6 && livingNeighbors <= 10;
                        else
                            Grid.NextGeneration[x][y][z] = livingNeighbors == 9 || livingNeighbors == 10;
                    }
                }
            }

            Grid.Tick();
            Generation++;
        }

        private Int32 LivingNeighborsOf(Int32 cellX, Int32 cellY, Int32 cellZ)
        {
            var total = 0;
            var xBegin = Math.Max(cellX - 1, 0);
            var xEnd = Math.Min(cellX + 2, Grid.EdgeSize);
            var yBegin = Math.Max(cellY - 1, 0);
            var yEnd = Math.Min(cellY + 2, Grid.EdgeSize);
            var zBegin = Math.Max(cellZ - 1, 0);
            var zEnd = Math.Min(cellZ + 2, Grid.EdgeSize);

            for (var x = xBegin; x < xEnd; x++)
                for (var y = yBegin; y < yEnd; y++)
                    for (var z = zBegin; z < zEnd; z++)
                        if (x != cellX || y != cellY || z != cellZ)
                            if (Grid.Cells[x][y][z])
                                total++;

            return total;
        }
    }
}