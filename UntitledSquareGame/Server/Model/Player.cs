using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Player : ICollidable
    {
        private const double MOVEMENT_SPEED = 3;

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;

        

        public Player(GameObjectType type)
        {
            this.Square = new Square(500, 250, 40, 40, type);
            this.Lives = 3;
        }

        public int Lives { get; set; }

        public Square Square
        {
            get;
        }

        public Direction ShootDirection
        {
            get;
            set;
        }

        public void SetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.moveUp = true;
                    break;
                case Direction.Down:
                    this.moveDown = true;
                    break;
                case Direction.Left:
                    this.moveLeft = true;
                    break;
                case Direction.Right:
                    this.moveRight = true;
                    break;
                case Direction.None:
                    break;
            }
        }

        public void ReleaseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.moveUp = false;
                    break;
                case Direction.Down:
                    this.moveDown = false;
                    break;
                case Direction.Left:
                    this.moveLeft = false;
                    break;
                case Direction.Right:
                    this.moveRight = false;
                    break;
                case Direction.None:
                    break;
            }
        }

        public Tuple<double, double> CalculateNewPosition()
        {
            double x = this.Square.X;
            double y = this.Square.Y;
            

            if (this.moveUp)
            {
                y -= MOVEMENT_SPEED;
            }

            if (this.moveDown)
            {
                y += MOVEMENT_SPEED;
            }

            if (this.moveLeft)
            {
                x -= MOVEMENT_SPEED;
            }

            if (this.moveRight)
            {
                x += MOVEMENT_SPEED;
            }

            return new Tuple<double, double>(x, y);
        }

        public void Move(double borderX, double borderY, double borderWidth, double borderHeight)
        {
            var newPosition = this.CalculateNewPosition();

            if (newPosition.Item1 > borderX && newPosition.Item1 + this.Square.Width < borderX + borderWidth)
            {
                this.Square.X = newPosition.Item1;
            }

            if (newPosition.Item2 > borderY && newPosition.Item2 + this.Square.Height < borderY + borderHeight)
            {
                this.Square.Y = newPosition.Item2;
            }
        }

        public Projectile GetProjectileOrNull()
        {
            if (this.ShootDirection != Direction.None)
            {
                var newProj = new Projectile(this.ShootDirection, this.Square.X + this.Square.Width / 2, this.Square.Y + this.Square.Height / 2);
                this.ResetShoot();
                return newProj;
            }

            return null;
        }

        public bool CollidesWith(IGameObject gameObject)
        {
            return this.Square.CollidesWith(gameObject.Square);
        }

        private void ResetShoot()
        {
            this.ShootDirection = Direction.None; 
        }
    }
}
