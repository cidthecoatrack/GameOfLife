using System;
using GameOfLife.Core;
using NUnit.Framework;

namespace GameOfLife.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        private Game game;

        [Test]
        public void SingleTickTimeOf100x100x100()
        {
            game = new Game(100);
            RunGame();
        }

        [Test]
        public void SingleTickTimeOf50x50x50()
        {
            game = new Game(50);
            RunGame();
        }

        [Test]
        public void SingleTickTimeOf10x10x10()
        {
            game = new Game(10);
            RunGame();
        }

        [Test]
        public void SingleTickTimeOf1x1x1()
        {
            game = new Game(1);
            RunGame();
        }

        private void RunGame()
        {
            var startTime = DateTime.Now;
            game.Tick();
            var duration = DateTime.Now.Subtract(startTime).Milliseconds;

            Assert.That(duration, Is.LessThan(1000));
        }
    }
}