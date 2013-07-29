using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Core
{
    public class Grid
    {
        public readonly Int32 XSize;
        public readonly Int32 YSize;

        public List<Cell> Cells { get; set; }

        public Grid(Int32 xSize, Int32 ySize)
        {
            if (xSize < 1 || ySize < 1)
                throw new IndexOutOfRangeException();

            XSize = xSize;
            YSize = ySize;

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            Cells = new List<Cell>();
            var randomizer = new Random();

            for (var x = 0; x < XSize; x++)
                for (var y = 0; y < YSize; y++)
                    Cells.Add(new Cell(x, y, Convert.ToBoolean(randomizer.Next(0, 2))));
        }

        public void SetLivingValues(IEnumerable<Boolean> newValues)
        {
            var count = newValues.Count();
            for (var i = 0; i < count; i++)
                Cells[i].AliveNextGeneration = newValues.ElementAt(i);

            Tick();
        }

        public void Tick()
        {
            foreach (var cell in Cells)
                cell.Tick();
        }
    }
}