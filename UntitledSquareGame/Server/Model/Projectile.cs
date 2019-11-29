using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Projectile : ICollidable
    {
        private const double MOVEMENT_SPEED = 10;

        private readonly Direction direction;

        public Projectile(Direction direction, double x, double y)
        {
            this.direction = direction;
            this.Square = new Square(x, y, 5, 5);
        }

        public Square Square
        {
            get;
        }

        public bool CollidesWith(IGameObject gameObject)
        {
            var firstPosA = new Tuple<double, double>(this.Square.X, this.Square.Y);
            var firstPosB = new Tuple<double, double>(this.Square.X + this.Square.Width, this.Square.Y + this.Square.Height);

            var secondPosA = new Tuple<double, double>(gameObject.Square.X, gameObject.Square.Y);
            var secondPosB = new Tuple<double, double>(gameObject.Square.X + gameObject.Square.Width, gameObject.Square.Y + gameObject.Square.Height);

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

        public void Move()
        {
            switch (this.direction)
            {
                case Direction.Up:
                    this.Square.Y -= MOVEMENT_SPEED;
                    break;
                case Direction.Down:
                    this.Square.Y += MOVEMENT_SPEED;
                    break;
                case Direction.Left:
                    this.Square.X -= MOVEMENT_SPEED;
                    break;
                case Direction.Right:
                    this.Square.X += MOVEMENT_SPEED;
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }
        }
    }
}
