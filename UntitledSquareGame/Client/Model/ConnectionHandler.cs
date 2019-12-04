using Client.EventArguments;
using SquareGameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ConnectionHandler
    {
        private const string START_MOVE_UP_COMMAND = "startmove:up";
        private const string START_MOVE_DOWN_COMMAND = "startmove:down";
        private const string START_MOVE_LEFT_COMMAND = "startmove:left";
        private const string START_MOVE_RIGHT_COMMAND = "startmove:right";
        private const string STOP_MOVE_UP_COMMAND = "stopmove:up";
        private const string STOP_MOVE_DOWN_COMMAND = "stopmove:down";
        private const string STOP_MOVE_LEFT_COMMAND = "stopmove:left";
        private const string STOP_MOVE_RIGHT_COMMAND = "stopmove:right";
        private const string SHOOT_UP_COMMAND = "shoot:up";
        private const string SHOOT_DOWN_COMMAND = "shoot:down";
        private const string SHOOT_LEFT_COMMAND = "shoot:left";
        private const string SHOOT_RIGHT_COMMAND = "shoot:right";

        private TcpClient client;
        private IPEndPoint endpoint;
        private bool listenForGameState;

        public ConnectionHandler(string ipAddress, int port)
        {
            this.endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            this.client = new TcpClient();
            this.client.Connect(this.endpoint);
        }

        public event EventHandler<GameStateReceivedEventArgs> GameStateReceived;

        public void Start()
        {
            var stream = client.GetStream();
            var message = Encoding.Default.GetBytes("Test");
            stream.Write(message, 0, message.Length);
            stream.Flush();
        }

        public async Task StartListeningForGameStateAsync()
        {
                await Task.Factory.StartNew(() => {
                    try
                    {
                        using (this.client)
                        {
                            this.listenForGameState = true;

                            var stream = client.GetStream();

                            while (this.listenForGameState)
                            {
                                var formatter = new BinaryFormatter();
                                var gameState = formatter.Deserialize(stream) as GameState;
                                this.FireGameStateReceivedReceived(new GameStateReceivedEventArgs(gameState));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                });
        }

        public Task StopListeningForGameStateAsync()
        {
            var task = Task.Factory.StartNew(() => {
                this.listenForGameState = false;
            });

            return task;
        }

        public void StartMoveUp()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(START_MOVE_UP_COMMAND);
            writer.Flush();
        }

        public void StartMoveDown()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(START_MOVE_DOWN_COMMAND);
            writer.Flush();
        }

        public void StartMoveLeft()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(START_MOVE_LEFT_COMMAND);
            writer.Flush();
        }

        public void StartMoveRight()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(START_MOVE_RIGHT_COMMAND);
            writer.Flush();
        }

        public void StopMoveUp()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(STOP_MOVE_UP_COMMAND);
            writer.Flush();
        }

        public void StopMoveDown()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(STOP_MOVE_DOWN_COMMAND);
            writer.Flush();
        }

        public void StopMoveLeft()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(STOP_MOVE_LEFT_COMMAND);
            writer.Flush();
        }

        public void StopMoveRight()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(STOP_MOVE_RIGHT_COMMAND);
            writer.Flush();
        }

        public void Shoot(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.SendShootCommand(SHOOT_UP_COMMAND);
                    break;
                case Direction.Down:
                    this.SendShootCommand(SHOOT_DOWN_COMMAND);
                    break;
                case Direction.Left:
                    this.SendShootCommand(SHOOT_LEFT_COMMAND);
                    break;
                case Direction.Right:
                    this.SendShootCommand(SHOOT_RIGHT_COMMAND);
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }
        }

        protected virtual void FireGameStateReceivedReceived(GameStateReceivedEventArgs args)
        {
            this.GameStateReceived?.Invoke(this, args);
        }

        private void SendShootCommand(string command)
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(command);
            writer.Flush();
        }
    }
}
