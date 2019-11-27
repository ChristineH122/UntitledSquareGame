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

        private const double BORDER_X = 2;
        private const double BORDER_Y = 2;
        private const double BORDER_WIDTH = 1086;
        private const double BORDER_HEIGHT = 596;

        public Game()
        {
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
            this.Searchers = new List<Searcher>();
        }

        public Game(Action<GameState> updatedStateAction)
        {
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
            this.Searchers = new List<Searcher>();
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

        public List<Searcher> Searchers
        {
            get;
        }

        public void Start()
        {
            this.Searchers.Add(new Searcher(this.FirstPlayer, 20, 20));

            while (true)
            {
                this.Render();
                this.CheckCollisions();

                this.updatedStateAction?.Invoke(this.BuildGameState());
                Thread.Sleep(10);
            }
        }

        private GameState BuildGameState()
        {
            return new GameState {
                PlayerOne = this.FirstPlayer.Square,
                PlayerTwo = this.SecondPlayer.Square,
                Searchers = this.Searchers.Select(s => s.Square).ToList() };
        }

        private void Render()
        {
            this.MovePlayers();
            this.MoveEnemies();
        }

        private bool CheckCollisions()
        {
            return true;
        }

        private void MoveEnemies()
        {
            foreach (var searcher in this.Searchers)
            {
                searcher.Move(BORDER_X, BORDER_Y, BORDER_WIDTH, BORDER_HEIGHT);
            }
        }

        private void MovePlayers()
        {
            this.FirstPlayer.Move(BORDER_X, BORDER_Y, BORDER_WIDTH, BORDER_HEIGHT);
            this.SecondPlayer.Move(BORDER_X, BORDER_Y, BORDER_WIDTH, BORDER_HEIGHT);
        }
    }
}
