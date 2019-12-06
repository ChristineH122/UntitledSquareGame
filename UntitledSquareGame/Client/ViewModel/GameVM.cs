using Client.Commands;
using Client.Model;
using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class GameVM : INotifyPropertyChanged

    {
        private const double MOVEMENT_SPEED = 3;
        private const string IP_ADDRESS = "127.0.0.1";
        private const int PORT = 5050;

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;
        private ConnectionHandler connectionHandler;
        private SquareVM square;
        private SquareVM secondPlayerSquare;
        private int playerOneLives;
        private int playerTwoLives;
        private int score;
        private bool gameOver;
        private string gameOverMessage;
        private bool hideGameOverMessage;
        private ObservableCollection<Square> gameObjects;

        public GameVM()
        {
            this.GameObjects = new ObservableCollection<Square>();
            this.Square = new SquareVM(new Square(500, 250, 40, 40, GameObjectType.Player1));
            this.SecondPlayerSquare = new SquareVM(new Square(500, 350, 40, 40, GameObjectType.Player2));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionHandler ConHandler
        {
            set;
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
        
        public ObservableCollection<Square> GameObjects
        {
            get
            {
                return this.gameObjects;
            }

            set
            {
                this.gameObjects = value;
                this.FireOnPropertyChanged();
            }
        }

        public int PlayerOneLives
        {
            get
            {
                return this.playerOneLives;
            }
            private set
            {
                this.playerOneLives = value;
                this.FireOnPropertyChanged();
            }
        }
        
        public int PlayerTwoLives
        {
            get
            {
                return this.playerTwoLives;
            }
            private set
            {
                this.playerTwoLives = value;
                this.FireOnPropertyChanged();
            }
        }

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool GameOver
        {
            get
            {
                return this.gameOver;
            }
            set
            {
                this.gameOver = value;
                this.FireOnPropertyChanged();
            }
        }

        public string GameOverMessage
        {
            get
            {
                return this.gameOverMessage;
            }
            set
            {
                this.gameOverMessage = value;
                this.FireOnPropertyChanged();
            }
        }

        public bool HideGameOverMessage
        {
            get
            {
                return this.hideGameOverMessage;
            }
            set
            {
                this.hideGameOverMessage = value;
                this.FireOnPropertyChanged();
            }
        }

        public async Task Start()
        {
            while (true)
            {
                try
                {
                    this.GameOverMessage = "Game over alder";
                    this.ConHandler = new ConnectionHandler(IP_ADDRESS, PORT);
                    this.ConHandler.GameStateReceived += ConHandler_GameStateReceived;
                    this.HideGameOverMessage = true;
                    await this.ConHandler.StartListeningForGameStateAsync();
                }
                catch (IOException e)
                {
                    this.GameOverMessage = e.Message;
                    this.GameOver = true;
                }
                catch (Exception e)
                {
                    var test = "hello";
                }
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

        protected virtual void FireOnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ConHandler_GameStateReceived(object sender, EventArguments.GameStateReceivedEventArgs e)
        {
            this.GameObjects = new ObservableCollection<Square>(e.GameState.GameObjects);
            this.PlayerOneLives = e.GameState.PlayerOneLives;
            this.PlayerTwoLives = e.GameState.PlayerTwoLives;
            this.Score = e.GameState.Score;
            this.GameOver = e.GameState.GameOver;
            this.HideGameOverMessage = !e.GameState.GameOver;

            if (this.GameOver)
                this.ConHandler.StopListeningForGameStateAsync();
        }
    }
}
