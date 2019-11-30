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
            this.Square = new Square(x, y, 5, 5, GameObjectType.Projectile);
        }

        public Square Square
        {
            get;
        }

        public bool CollidesWith(IGameObject gameObject)
        {
            return this.Square.CollidesWith(gameObject.Square);
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
