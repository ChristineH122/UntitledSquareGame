using Client.Commands;
using Client.Enums;
using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace Client.ViewModel
{
    public class GameVM : INotifyPropertyChanged

    {
        private const double MOVEMENT_SPEED = 3; 

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;
        private ConnectionHandler connectionHandler;
        private SquareVM square;

        public GameVM()
        {
            this.ConHandler = new ConnectionHandler("127.0.0.1", 5050);
            this.ConHandler.StartListeningForGameStateAsync();
            this.ConHandler.GameStateReceived += ConHandler_GameStateReceived;
            this.Square = new SquareVM(new Square(500, 250, 40, 40));
            //this.connectionHandler = new ConnectionHandler("192.168.178.20", 5050);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ConHandler_GameStateReceived(object sender, EventArguments.GameStateReceivedEventArgs e)
        {
            this.Square.X = e.GameState.PlayerOne.X;
            this.Square.Y = e.GameState.PlayerOne.Y;
        }

        public ConnectionHandler ConHandler
        {
            get;
        }

        public SquareVM Square
        {
            get
            {
                return this.square;
            }

            private set
            {
                this.square = value;
                this.FireOnPropertyChanged();
            }
        }

        public void ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.ConHandler.MoveUp();
                    break;

                case Direction.Down:
                    this.ConHandler.MoveDown();
                    break;

                case Direction.Left:
                    this.ConHandler.MoveLeft();
                    break;

                case Direction.Right:
                    this.ConHandler.MoveRight();
                    break;
            }
        }

        public void ReleaseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.ConHandler.MoveUp();
                    break;

                case Direction.Down:
                    this.ConHandler.MoveDown();
                    break;

                case Direction.Left:
                    this.ConHandler.MoveLeft();
                    break;

                case Direction.Right:
                    this.ConHandler.MoveRight();
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

        protected virtual void FireOnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
