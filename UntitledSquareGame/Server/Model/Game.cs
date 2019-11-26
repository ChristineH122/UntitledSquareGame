using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Game
    {
        private readonly Action<GameState> updatedStateAction;

        public Game()
        {
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
        }

        public Game(Action<GameState> updatedStateAction)
        {
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
            this.updatedStateAction = updatedStateAction ?? throw new ArgumentNullException(nameof(updatedStateAction));
        }

        public Player FirstPlayer
        {
            get;
        }

        public Player SecondPlayer
        {
            get;
        }

        public void Start()
        {
            while (true)
            {
                this.Render();

                this.updatedStateAction?.Invoke(this.BuildGameState());
                Thread.Sleep(10);
            }
        }

        private GameState BuildGameState()
        {
            return new GameState { PlayerOne = this.FirstPlayer.Square };
        }

        private void MoveEnemies()
        {

        }

        private void MovePlayers()
        {
            this.FirstPlayer.Move();
            this.SecondPlayer.Move();
        }
    }
}
