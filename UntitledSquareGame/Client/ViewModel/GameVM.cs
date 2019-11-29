﻿using Client.Commands;
using Client.Model;
using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private SquareVM secondPlayerSquare;
        private ObservableCollection<Square> searchers;

        public GameVM()
        {
            this.Searchers = new ObservableCollection<Square>();
            this.ConHandler = new ConnectionHandler("127.0.0.1", 5050);
            this.ConHandler.StartListeningForGameStateAsync();
            this.ConHandler.GameStateReceived += ConHandler_GameStateReceived;
            this.Square = new SquareVM(new Square(500, 250, 40, 40));
            this.SecondPlayerSquare = new SquareVM(new Square(500, 350, 40, 40));
            //this.connectionHandler = new ConnectionHandler("192.168.178.20", 5050);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                //this.FireOnPropertyChanged();
            }
        }

        public SquareVM SecondPlayerSquare
        {
            get
            {
                return this.secondPlayerSquare;
            }

            private set
            {
                this.secondPlayerSquare = value;
                //this.FireOnPropertyChanged();
            }
        }
        
        public ObservableCollection<Square> Searchers
        {
            get
            {
                return this.searchers;
            }

            set
            {
                this.searchers = value;
                this.FireOnPropertyChanged();
            }
        }

        public void ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.ConHandler.StartMoveUp();
                    break;

                case Direction.Down:
                    this.ConHandler.StartMoveDown();
                    break;

                case Direction.Left:
                    this.ConHandler.StartMoveLeft();
                    break;

                case Direction.Right:
                    this.ConHandler.StartMoveRight();
                    break;
            }
        }

        public void ReleaseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.ConHandler.StopMoveUp();
                    break;

                case Direction.Down:
                    this.ConHandler.StopMoveDown();
                    break;

                case Direction.Left:
                    this.ConHandler.StopMoveLeft();
                    break;

                case Direction.Right:
                    this.ConHandler.StopMoveRight();
                    break;
            }
        }

        public void Shoot(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.ConHandler.Shoot(Direction.Up);
                    break;
                case Direction.Down:
                    this.ConHandler.Shoot(Direction.Down);
                    break;
                case Direction.Left:
                    this.ConHandler.Shoot(Direction.Left);
                    break;
                case Direction.Right:
                    this.ConHandler.Shoot(Direction.Right);
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }
        }

        public void Move()
        {
            //if (this.moveUp)
            //{
            //    this.Square.Y -= MOVEMENT_SPEED;
            //}

            //if (this.moveDown)
            //{
            //    this.Square.Y += MOVEMENT_SPEED;
            //}

            //if (this.moveLeft)
            //{
            //    this.Square.X -= MOVEMENT_SPEED;
            //}

            //if (this.moveRight)
            //{
            //    this.Square.X += MOVEMENT_SPEED;
            //}
        }

        protected virtual void FireOnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ConHandler_GameStateReceived(object sender, EventArguments.GameStateReceivedEventArgs e)
        {
            this.Square.X = e.GameState.PlayerOne.X;
            this.Square.Y = e.GameState.PlayerOne.Y;
            this.SecondPlayerSquare.X = e.GameState.PlayerTwo.X;
            this.SecondPlayerSquare.Y = e.GameState.PlayerTwo.Y;
            this.Searchers = new ObservableCollection<Square>(e.GameState.Searchers);
        }
    }
}
