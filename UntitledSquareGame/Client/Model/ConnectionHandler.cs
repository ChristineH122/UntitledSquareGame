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
        private const string MOVE_UP_COMMAND = "move:up";
        private const string MOVE_DOWN_COMMAND = "move:down";
        private const string MOVE_LEFT_COMMAND = "move:left";
        private const string MOVE_RIGHT_COMMAND = "move:right";

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
            // stream.Close();
        }

        public Task StartListeningForGameStateAsync()
        {
            var task = Task.Factory.StartNew(() => {
                this.listenForGameState = true;

                var stream = client.GetStream();

                while (this.listenForGameState)
                {
                    var formatter = new BinaryFormatter();
                    var gameState = formatter.Deserialize(stream) as GameState;
                    this.FireGameStateReceivedReceived(new GameStateReceivedEventArgs(gameState));
                }
            });

            return task;
        }

        public Task StopListeningForGameStateAsync()
        {
            var task = Task.Factory.StartNew(() => {
                this.listenForGameState = false;
            });

            return task;
        }

        public void MoveUp()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(MOVE_UP_COMMAND);
            writer.Flush();
        }

        public void MoveDown()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(MOVE_DOWN_COMMAND);
            writer.Flush();
        }

        public void MoveLeft()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(MOVE_LEFT_COMMAND);
            writer.Flush();
        }

        public void MoveRight()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(MOVE_RIGHT_COMMAND);
            writer.Flush();
        }

        protected virtual void FireGameStateReceivedReceived(GameStateReceivedEventArgs args)
        {
            this.GameStateReceived?.Invoke(this, args);
        }
    }
}
