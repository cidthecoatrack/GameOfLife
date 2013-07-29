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
            var x = randomizer.Next(1, 1001);
            var y = randomizer.Next(1, 1001);

            Grid = new Grid(x, y);
            Generation = 1;
        }

        public Game(Int32 xSize, Int32 ySize)
        {
            Grid = new Grid(xSize, ySize);
            Generation = 1;
        }

        public void Reset()
        {
            Grid = new Grid(Grid.XSize, Grid.YSize);
            Generation = 1;
        }

        public void SetLivingValues(Boolean[][] newValues)
        {
            var countToCompare = newValues[0].Length;
            foreach (var cellRow in newValues)
                if (countToCompare != cellRow.Length)
                    throw new ArgumentOutOfRangeException();

            if (newValues.Length != Grid.XSize || newValues[0].Length != Grid.YSize)
                throw new ArgumentOutOfRangeException();

            Grid.SetLivingValues(newValues);
        }

        public void Execute(Int32 numberOfTicks)
        {
            while (numberOfTicks-- > 0)
                Tick();
        }

        public void Tick()
        {
            for (var x = 0; x < Grid.XSize; x++)
            {
                for (var y = 0; y < Grid.YSize; y++)
                {
                    var cell = Grid.Cells[x][y];

                    if (cell.Alive)
                    {
                        CheckUnderpopulation(cell);
                        CheckOverpopulation(cell);
                    }
                    else
                    {
                        CheckReproduction(cell);
                    }
                }
            }

            Grid.Tick();
            Generation++;
        }

        private void CheckUnderpopulation(Cell cell)
        {
            if (livingNeighborsOf(cell) < 2)
                cell.AliveNextGeneration = false;
        }

        private void CheckOverpopulation(Cell cell)
        {
            if (livingNeighborsOf(cell) > 3)
                cell.AliveNextGeneration = false;
        }

        private void CheckReproduction(Cell cell)
        {
            if (livingNeighborsOf(cell) == 3)
                cell.AliveNextGeneration = true;
        }

        private Int32 livingNeighborsOf(Cell cell)
        {
            var total = 0;
            var xBegin = Math.Max(cell.X - 1, 0);
            var xEnd = Math.Min(cell.X + 2, Grid.XSize);
            var yBegin = Math.Max(cell.Y - 1, 0);
            var yEnd = Math.Min(cell.Y + 2, Grid.YSize);

            for (var x = xBegin; x < xEnd; x++)
                for (var y = yBegin; y < yEnd; y++)
                    if (x != cell.X || y != cell.Y)
                        if (Grid.Cells[x][y].Alive)
                            total++;

            return total;
        }
    }
}