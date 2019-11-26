using Client.Commands;
using Client.Enums;
using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.ViewModel
{
    public class GameVM
    {
        private const double MOVEMENT_SPEED = 3; 

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;
        private ConnectionHandler connectionHandler;

        public GameVM()
        {
            this.ConHandler = new ConnectionHandler("192.168.174.113", 5050);
            this.Square = new SquareVM(new Square(500, 250, 40, 40));
            //this.connectionHandler = new ConnectionHandler("192.168.178.20", 5050);
        }

        public ConnectionHandler ConHandler
        {
            get;
        }

        public SquareVM Square
        {
            get; set;
        }

        public void ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    //this.moveUp = true;
                    this.ConHandler.MoveUp();
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
