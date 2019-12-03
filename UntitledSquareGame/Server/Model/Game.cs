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

        private const double BORDER_X = 0;
        private const double BORDER_Y = 0;
        private const double BORDER_WIDTH = 1087;
        private const double BORDER_HEIGHT = 597;

        private Random random;

        public Game()
        {
            this.random = new Random();
            this.FirstPlayer = new Player(GameObjectType.Player1);
            this.SecondPlayer = new Player(GameObjectType.Player2);
            this.Searchers = new List<Searcher>();
            this.Projectiles = new List<Projectile>();
            this.Score = 0;
        }

        public Game(Action<GameState> updatedStateAction)
        {
            this.random = new Random();
            this.FirstPlayer = new Player(GameObjectType.Player1);
            this.SecondPlayer = new Player(GameObjectType.Player2);
            this.Searchers = new List<Searcher>();
            this.Projectiles = new List<Projectile>();
            this.Score = 0;
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

        public int Score
        {
            get;
            private set;
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
            while (!this.IsGameOver())
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
                PlayerOneLives = this.FirstPlayer.Lives,
                PlayerTwoLives = this.SecondPlayer.Lives,
                GameObjects = this.Searchers.Select(s => s.Square).Concat(this.Projectiles.Select(s => s.Square)).Prepend(this.FirstPlayer.Square).Prepend(this.SecondPlayer.Square).ToList(),
                Score = this.Score,
                GameOver = this.IsGameOver()
            };
        }

        private void Render()
        {
            this.MoveProjectiles();
            this.PlaceProjectiles();
            this.MovePlayers();
            this.MoveEnemies();
            this.RemoveOuterProjectiles();
        }

        private void ResolveCollisions()
        {
            this.ResolveProjectileCollisions();
            this.ResolveSearcherCollisions();
        }

        private void PlaceProjectiles()
        {
            var proj1 = this.FirstPlayer.GetProjectileOrNull();
            var proj2 = this.SecondPlayer.GetProjectileOrNull();

            if (proj1 != null)
            {
                this.Projectiles.Add(proj1);
            }

            if (proj2 != null)
            {
                this.Projectiles.Add(proj2);
            }
        }

        private void ResolveProjectileCollisions()
        {
            var updatedSearcherList = new List<Searcher>();
            var updatedProjectileList = new List<Projectile>();
            var hitProjectileList = new List<Projectile>();

            bool isHit = false;


            foreach (var searcher in this.Searchers)
            {
                foreach (var projectile in this.Projectiles)
                {

                    if (projectile.CollidesWith(searcher))
                    {
                        if (!hitProjectileList.Contains(projectile))
                        {
                            hitProjectileList.Add(projectile);
                        }
                        isHit = true;
                    }
                }

                if (!isHit)
                {
                    updatedSearcherList.Add(searcher);                   
                }
                else
                {
                    this.Score += 1;
                }

                isHit = false;
            }

            this.Projectiles = this.Projectiles.Except(hitProjectileList).ToList();
            

            this.Searchers = updatedSearcherList;
        }

        private void ResolveSearcherCollisions()
        {
            var newSearcherList = new List<Searcher>();

            foreach (var searcher in this.Searchers)
            {
                if (this.FirstPlayer.CollidesWith(searcher))
                {
                    this.FirstPlayer.Lives--;
                }
                else if (this.SecondPlayer.CollidesWith(searcher))
                {
                    this.SecondPlayer.Lives--;
                }
                else
                {
                    newSearcherList.Add(searcher);
                }
            }

            this.Searchers = newSearcherList;
        }

        private bool IsGameOver()
        {
            return (this.FirstPlayer.Lives <= 0 || this.SecondPlayer.Lives <= 0);
        }

        private void RemoveOuterProjectiles()
        {
            this.Projectiles = this.Projectiles.Where(p => {
                if (p.Square.X < 0 || p.Square.X > BORDER_WIDTH)
                {
                    return false;
                }

                if (p.Square.Y < 0 || p.Square.Y > BORDER_HEIGHT)
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
            foreach(var projectile in this.Projectiles)
            {
                projectile.Move();
            }
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
