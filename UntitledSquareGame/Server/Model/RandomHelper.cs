using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public static class RandomHelper
    {
        public static Direction GetRandomDirection(Random random)
        {
            int randInt = random.Next(1, 5);

            switch (randInt)
            {
                case 1:
                    return Direction.Up;

                case 2:
                    return Direction.Down;

                case 3:
                    return Direction.Left;

                case 4:
                    return Direction.Right;

                default:
                    return Direction.Left;
            }
        }
    }
}
