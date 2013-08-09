using System;
using System.ComponentModel;

namespace GameOfLife.Core
{
    public class Game : INotifyPropertyChanged
    {
        private const Int32 maxSize = 100;
        private const Int32 minSize = 1;

        public Grid Grid { get; private set; }
        public Int32 MinLive { get; set; }
        public Int32 MaxLive { get; set; }
        public Int32 MinBorn { get; set; }
        public Int32 MaxBorn { get; set; }

        public Int32 EdgeSize
        {
            get { return edgeSize; }
            set
            {
                if (value >= minSize && value <= maxSize)
                {
                    edgeSize = value;
                    Reset();
                }
            }
        }


        public Int32 Generation
        {
            get { return generation; }
            set
            {
                generation = value;
                OnPropertyChanged("Generation");
            }
        }

        public Boolean NotRunning
        {
            get { return notRunning; }
            set
            {
                notRunning = value;
                OnPropertyChanged("NotRunning");
            }
        }

        private Int32 edgeSize;
        private Int32 generation;
        private Boolean notRunning;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public Game()
        {
            edgeSize = 15;
            InitializeRules();
            Reset();
        }

        private void InitializeRules()
        {
            MinLive = 2;
            MaxLive = 3;
            MinBorn = 3;
            MaxBorn = 3;
        }

        public Game(Int32 edgeSize)
        {
            this.edgeSize = edgeSize;
            InitializeRules();
            Reset();
        }

        public void Reset()
        {
            Grid = new Grid(edgeSize);
            Generation = 1;
            NotRunning = true;
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
            NotRunning = true;

            for (var x = 0; x < Grid.EdgeSize; x++)
            {
                for (var y = 0; y < Grid.EdgeSize; y++)
                {
                    for (var z = 0; z < Grid.EdgeSize; z++)
                    {
                        var livingNeighbors = LivingNeighborsOf(x, y, z);

                        if (Grid.Cells[x][y][z])
                            Grid.NextGeneration[x][y][z] = livingNeighbors >= MinLive && livingNeighbors <= MaxLive;
                        else
                            Grid.NextGeneration[x][y][z] = livingNeighbors >= MinBorn && livingNeighbors <= MaxBorn;

                        if (Grid.Cells[x][y][z] != Grid.NextGeneration[x][y][z])
                            NotRunning = false;
                    }
                }
            }

            if (!NotRunning)
            {
                Grid.Tick();
                Generation++;
            }
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