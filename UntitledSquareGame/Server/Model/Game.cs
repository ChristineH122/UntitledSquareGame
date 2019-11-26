using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Game
    {
        public Game()
        {
            this.FirstPlayer = new Player();
            this.SecondPlayer = new Player();
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
                this.MovePlayers();
                this.MoveEnemies();
            }
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
