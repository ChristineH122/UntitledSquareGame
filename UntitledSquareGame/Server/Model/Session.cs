using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Session
    {
        public Session(User firstUser)
        {
            this.Game = new Game();
            
            this.FirstPlayer = firstUser;

            var task = Task.Factory.StartNew(this.ListenFirstPlayerCommand);
            this.Game.Start();
        }

        public User FirstPlayer
        {
            get;
        }

        public Game Game
        {
            get;
        }

        public void ListenFirstPlayerCommand()
        {
            var stream = this.FirstPlayer.Client.GetStream();

            while (true)
            {
                var buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                var message = Encoding.ASCII.GetString(buffer);
                this.HandlePlayerCommnad(this.Game.FirstPlayer, message);
            }
        }

        public void HandlePlayerCommnad(Player player, string message)
        {
            var splitMessage = message.Split(':');
            var command = splitMessage[0];
            var parameter = splitMessage[1];

            if (command == "move")
            {
                switch (parameter)
                {
                    case "up":
                        player.SetDirection(Direction.Up);
                        break;
                    case "down":
                        player.SetDirection(Direction.Down);
                        break;
                    case "left":
                        player.SetDirection(Direction.Left);
                        break;
                    case "right":
                        player.SetDirection(Direction.Right);
                        break;
                    default:
                        break;
                }
            }

            if (command == "shoot")
            {

            }
        }
    }
}
