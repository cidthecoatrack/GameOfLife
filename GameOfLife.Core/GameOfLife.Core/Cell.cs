using System;

namespace GameOfLife.Core
{
    public class Cell
    {
        public readonly Int32 X;
        public readonly Int32 Y;

        public Boolean Alive { get; private set; }
        public Boolean AliveNextGeneration { get; set; }

        public Cell(Int32 x, Int32 y, Boolean alive)
        {
            X = x;
            Y = y;
            Alive = alive;
        }

        public void Tick()
        {
            Alive = AliveNextGeneration;
        }
    }
}