using System;

namespace GameOfLife.Core
{
    public class Grid
    {
        public readonly Int32 XSize;
        public readonly Int32 YSize;

        public Cell[][] Cells { get; set; }

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
            var randomizer = new Random();
            Cells = new Cell[XSize][];

            for (var x = 0; x < XSize; x++)
            {
                Cells[x] = new Cell[YSize];

                for (var y = 0; y < YSize; y++)
                    Cells[x][y] = new Cell(x, y, Convert.ToBoolean(randomizer.Next(0, 2)));
            }
        }

        public void SetLivingValues(Boolean[][] newValues)
        {
            for (var x = 0; x < XSize; x++)
                for (var y = 0; y < YSize; y++)
                    Cells[x][y].AliveNextGeneration = newValues[x][y];

            Tick();
        }

        public void Tick()
        {
            for (var x = 0; x < XSize; x++)
                for (var y = 0; y < YSize; y++)
                    Cells[x][y].Tick();
        }
    }
}