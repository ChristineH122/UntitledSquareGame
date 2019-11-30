using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGameObjects
{
    [Serializable()]
    public class Square
    {
        public Square(double x, double y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public double X
        {
            get; set;
        }

        public double Y
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }

        public bool CollidesWith(Square square)
        {
            var firstPosA = new Tuple<double, double>(this.X, this.Y);
            var firstPosB = new Tuple<double, double>(this.X + this.Width, this.Y + this.Height);

            var secondPosA = new Tuple<double, double>(square.X, square.Y);
            var secondPosB = new Tuple<double, double>(square.X + square.Width, square.Y + square.Height);

            if (firstPosA.Item1 > secondPosB.Item1 || firstPosB.Item1 < secondPosA.Item1)
            {
                return false;
            }

            if (firstPosA.Item2 > secondPosB.Item2 || firstPosB.Item2 < secondPosA.Item2)
            {
                return false;
            }

            return true;
        }
    }
}
