using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Session
    {
        private Task listenFirstPlayer;
        private Task listenSecondPlayer;
        private Task playGame;

        // TODO delete
        public Session(User firstUser)
        {
            this.Game = new Game(this.SendGameStateToClients);

            this.FirstUser = firstUser;

            var task = Task.Factory.StartNew(this.ListenFirstPlayerCommand);
            this.Game.Start();
        }

        public Session(User firstUser, User secondUser)
        {
            this.Game = new Game(this.SendGameStateToClients);
            
            this.FirstUser = firstUser;
            this.SecondUser = secondUser;

            
        }

        public User FirstUser
        {
            get;
        }

        public User SecondUser
        {
            get;
        }

        public Game Game
        {
            get;
        }

        public async void StartAsync()
        {
            
            this.listenFirstPlayer = Task.Factory.StartNew(this.ListenFirstPlayerCommand);
            this.listenSecondPlayer = Task.Factory.StartNew(this.ListenSecondPlayerCommand);
            this.playGame = Task.Factory.StartNew(this.Game.Start);
        }

        public void Stop()
        {
            this.listenFirstPlayer.Dispose();
            this.listenSecondPlayer.Dispose();
            this.playGame.Dispose();
        }

        public void ListenFirstPlayerCommand()
        {
            var stream = this.FirstUser.Client.GetStream();

            try
            {
                while (true)
                {
                    var buffer = new byte[1024];
                    var bytes = stream.Read(buffer, 0, buffer.Length);
                    var message = Encoding.ASCII.GetString(buffer, 0, bytes);
                    this.HandlePlayerCommand(this.Game.FirstPlayer, message);
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ListenSecondPlayerCommand()
        {
            var stream = this.SecondUser.Client.GetStream();

            try
            {
                while (true)
                {
                    var buffer = new byte[1024];
                    var bytes = stream.Read(buffer, 0, buffer.Length);
                    var message = Encoding.ASCII.GetString(buffer, 0, bytes);
                    this.HandlePlayerCommand(this.Game.SecondPlayer, message);
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void HandlePlayerCommand(Player player, string message)
        {
            var splitMessage = message.Split(':');
            var command = splitMessage[0];
            var parameter = splitMessage[1];

            if (command == "startmove")
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

            if (command == "stopmove")
            {
                switch (parameter)
                {
                    case "up":
                        player.ReleaseDirection(Direction.Up);
                        break;
                    case "down":
                        player.ReleaseDirection(Direction.Down);
                        break;
                    case "left":
                        player.ReleaseDirection(Direction.Left);
                        break;
                    case "right":
                        player.ReleaseDirection(Direction.Right);
                        break;
                    default:
                        break;
                }
            }

            if (command == "shoot")
            {
                var x = player.Square.X + player.Square.Width/2;
                var y = player.Square.Y + player.Square.Height/2;

                switch (parameter)
                {
                    case "up":
                        player.ShootDirection = Direction.Up;
                        break;
                    case "down":
                        player.ShootDirection = Direction.Down;
                        break;
                    case "left":
                        player.ShootDirection = Direction.Left;
                        break;
                    case "right":
                        player.ShootDirection = Direction.Right;
                        break;
                    default:
                        break;
                }
            }
        }

        public void SendGameStateToClients(GameState state)
        {
            var data = this.SerializeGameStateToByteArray(state);
            var firstUserStream = this.FirstUser.Client.GetStream();
            var secondUserStream = this.SecondUser.Client.GetStream();
        
            firstUserStream.Write(data, 0, data.Length);
            secondUserStream.Write(data, 0, data.Length);
            firstUserStream.Flush();
            secondUserStream.Flush();
        }

        private byte[] SerializeGameStateToByteArray(GameState gameState)
        {
            var formatter = new BinaryFormatter();
            byte[] data;
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, gameState);
                data = ms.ToArray();
            }

            return data;
        }
    }
}
