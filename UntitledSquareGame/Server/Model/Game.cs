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

        private Random random;

        public Game()
        {
            this.random = new Random();
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
            this.Searchers = new List<Searcher>();
            this.Projectiles = new List<Projectile>();
        }

        public Game(Action<GameState> updatedStateAction)
        {
            this.random = new Random();
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
            this.Searchers = new List<Searcher>();
            this.Projectiles = new List<Projectile>();
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
            set;
        }

        public List<Projectile> Projectiles
        {
            get;
            set;
        }

        public void Start()
        {
            while (true)
            {
                this.SpawnNewEnemy();
                this.Render();
                this.ResolveCollisions();

                this.updatedStateAction?.Invoke(this.BuildGameState());
                Thread.Sleep(10);
            }
        }

        private GameState BuildGameState()
        {
            return new GameState
            {
                PlayerOne = this.FirstPlayer.Square,
                PlayerOneLives = this.FirstPlayer.Lives,
                PlayerTwo = this.SecondPlayer.Square,
                PlayerTwoLives = this.SecondPlayer.Lives,
                Searchers = this.Searchers.Select(s => s.Square).ToList(),
                Projectiles = this.Projectiles.Select(s => s.Square).ToList()
            };
        }

        private void Render()
        {
            this.MoveProjectiles();
            this.MovePlayers();
            this.MoveEnemies();
            this.RemoveOuterProjectiles();
        }

        private void ResolveCollisions()
        {
            this.ResolveProjectileCollisions();
            this.ResolveSearcherCollisions();
        }

        private void ResolveProjectileCollisions()
        {
            var updatedSearcherList = new List<Searcher>();

            foreach (var projectile in this.Projectiles)
            {
                foreach (var searcher in this.Searchers)
                {
                    if (!projectile.CollidesWith(searcher))
                    {
                        updatedSearcherList.Add(searcher);
                    }
                }
            }
        }

        private void ResolveSearcherCollisions()
        {
            var newSearcherList = new List<Searcher>();

            foreach (var searcher in this.Searchers)
            {
                if (this.FirstPlayer.CollidesWith(searcher))
                {
                    this.FirstPlayer.Lives -= 1;
                }
                else
                {
                    newSearcherList.Add(searcher);
                }
            }

            this.Searchers = newSearcherList;
        }

        private void RemoveOuterProjectiles()
        {
            this.Projectiles = this.Projectiles.Where(p => {
                if (p.Square.X < 0 || p.Square.X > BORDER_WIDTH)
                {
                    return false;
                }

                if (p.Square.Y < 0 || p.Square.X > BORDER_HEIGHT)
                {
                    return false;
                }

                return true;
            }).ToList();
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

        private void MoveProjectiles()
        {
            
        }

        private void SpawnNewEnemy()
        {
            int spawnChance = 50 * this.Searchers.Count + 1;

            int shouldSpawn = this.random.Next(0, spawnChance);

            if(shouldSpawn < spawnChance - 1)
            {
                return;
            }

            Searcher newSearcher = null;
            Player target = null;
            Direction spawnDirection = Direction.None;
            int targetNumber = this.random.Next(1, 3);
            int x = 0;
            int y = 0;

            spawnDirection = RandomHelper.GetRandomDirection(this.random);

            switch(spawnDirection)
            {
                case Direction.Up:
                    x = this.random.Next(0, 1046);
                    y = -45;
                    break;

                case Direction.Down:
                    x = this.random.Next(0, 1046);
                    y = 640;
                    break;

                case Direction.Left:
                    x = -45;
                    y = this.random.Next(0, 546);
                    break;

                case Direction.Right:
                    x = 1130;
                    y = this.random.Next(0, 546);
                    break;
            }

            switch(targetNumber)
            {
                case 1:
                    target = this.FirstPlayer;
                    break;

                case 2:
                    target = this.SecondPlayer;
                    break;
            }

            newSearcher = new Searcher(target, x, y);

            this.Searchers.Add(newSearcher);
        }
    }
}
