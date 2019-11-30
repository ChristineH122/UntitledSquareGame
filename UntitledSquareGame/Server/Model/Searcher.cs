using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Searcher : ICollidable
    {
        private const double MOVEMENT_SPEED = 2;

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;

        public Searcher(Player target, double x, double y)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Square = new Square(x, y, 40, 40, GameObjectType.Searcher);
        }

        public Player Target
        {
            get;
        }

        public Square Square
        {
            get;
        }


        public void Move(double borderX, double borderY, double borderWidth, double borderHeight)
        {
            this.SetDirection();

            var newPosition = this.CalculateNewPosition();

            //if (newPosition.Item1 > borderX && newPosition.Item1 + this.Square.Width < borderX + borderWidth)
            //{
            //    this.Square.X = newPosition.Item1;
            //}

            //if (newPosition.Item2 > borderY && newPosition.Item2 + this.Square.Height < borderY + borderHeight)
            //{
            //    this.Square.Y = newPosition.Item2;
            //}

            this.Square.X = newPosition.Item1;
            this.Square.Y = newPosition.Item2;
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

        private void SetDirection()
        {
            this.ResetDirection();

            var targetX = this.Target.Square.X;
            var targetY = this.Target.Square.Y;
            
            if (this.Square.X >= targetX) { this.moveLeft = true; }
            if (this.Square.X < targetX) { this.moveRight = true; }
            if (this.Square.Y >= targetY) { this.moveUp = true; }
            if (this.Square.Y < targetY) { this.moveDown = true; }
        }

        private void ResetDirection()
        {
            this.moveUp = false;
            this.moveDown = false;
            this.moveLeft = false;
            this.moveRight = false;
        }

        public bool CollidesWith(IGameObject gameObject)
        {
            return this.Square.CollidesWith(gameObject.Square);
        }
    }
}
