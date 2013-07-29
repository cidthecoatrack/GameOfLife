using System;
using System.Collections.Generic;
using System.Linq;

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

        public void SetLivingValues(IEnumerable<Boolean> newValues)
        {
            if (newValues.Count() != Grid.Cells.Count)
                throw new ArgumentOutOfRangeException();

            Grid.SetLivingValues(newValues);

        }

        public void Tick()
        {
            CheckUnderpopulation();
            CheckOverpopulation();
            CheckReproduction();

            Grid.Tick();
            Generation++;
        }

        private void CheckReproduction()
        {
            foreach (var cell in Grid.Cells.Where(c => !c.Alive))
                if (livingNeighborsOf(cell) == 3)
                    cell.AliveNextGeneration = true;
        }

        private void CheckOverpopulation()
        {
            foreach (var cell in Grid.Cells.Where(c => c.Alive))
                if (livingNeighborsOf(cell) > 3)
                    cell.AliveNextGeneration = false;
        }

        private void CheckUnderpopulation()
        {
            foreach (var cell in Grid.Cells.Where(c => c.Alive))
                if (livingNeighborsOf(cell) < 2)
                    cell.AliveNextGeneration = false;
        }

        private Int32 livingNeighborsOf(Cell cell)
        {
            return Grid.Cells.Count(c => c.Alive && c != cell
                                      && Math.Abs(c.X - cell.X) <= 1
                                      && Math.Abs(c.Y - cell.Y) <= 1);
        }
    }
}