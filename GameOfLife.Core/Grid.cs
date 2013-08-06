using System;

namespace GameOfLife.Core
{
    public class Grid
    {
        public readonly Int32 EdgeSize;

        public Boolean[][][] Cells { get; private set; }
        public Boolean[][][] NextGeneration { get; set; }

        public Grid(Int32 edgeSize)
        {
            if (edgeSize < 1)
                throw new IndexOutOfRangeException();

            EdgeSize = edgeSize;

            InitializeGrids();
        }

        private void InitializeGrids()
        {
            var randomizer = new Random();
            Cells = new Boolean[EdgeSize][][];
            NextGeneration = new Boolean[EdgeSize][][];

            for (var x = 0; x < EdgeSize; x++)
            {
                Cells[x] = new Boolean[EdgeSize][];
                NextGeneration[x] = new Boolean[EdgeSize][];

                for (var y = 0; y < EdgeSize; y++)
                {
                    Cells[x][y] = new Boolean[EdgeSize];
                    NextGeneration[x][y] = new Boolean[EdgeSize];

                    for(var z = 0; z < EdgeSize; z++)
                        Cells[x][y][z] = Convert.ToBoolean(randomizer.Next(0, 2));
                }
            }
        }

        public void SetLivingValues(Boolean[][][] newValues)
        {
            for (var x = 0; x < EdgeSize; x++)
                for (var y = 0; y < EdgeSize; y++)
                    for (var z = 0; z < EdgeSize; z++)
                        Cells[x][y][z] = newValues[x][y][z];
        }

        public void Tick()
        {
            for (var x = 0; x < EdgeSize; x++)
                for (var y = 0; y < EdgeSize; y++)
                    for (var z = 0; z < EdgeSize; z++)
                        Cells[x][y][z] = NextGeneration[x][y][z];
        }
    }
}