using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Player
    {
        private const double MOVEMENT_SPEED = 3;

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;

        public Player()
        {
            this.Square = new Square(500, 250, 40, 40);
        }

        public Square Square
        {
            get;
        }

        public void SetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.moveUp = !this.moveUp;
                    break;
                case Direction.Down:
                    this.moveDown = !this.moveDown;
                    break;
                case Direction.Left:
                    this.moveLeft = !this.moveLeft;
                    break;
                case Direction.Right:
                    this.moveRight = !this.moveRight;
                    break;
                case Direction.None:
                    break;
            }
        }

        public void Move()
        {
            if (this.moveUp)
            {
                this.Square.Y -= MOVEMENT_SPEED;
            }

            if (this.moveDown)
            {
                this.Square.Y += MOVEMENT_SPEED;
            }

            if (this.moveLeft)
            {
                this.Square.X -= MOVEMENT_SPEED;
            }

            if (this.moveRight)
            {
                this.Square.X += MOVEMENT_SPEED;
            }
        }
    }
}
