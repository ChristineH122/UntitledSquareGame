﻿using SquareGameObjects;
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
        public Session(User firstUser)
        {
            this.Game = new Game(this.SendGameStateToClients);
            
            this.FirstUser = firstUser;

            var task = Task.Factory.StartNew(this.ListenFirstPlayerCommand);
            this.Game.Start();
        }

        public User FirstUser
        {
            get;
        }

        public Game Game
        {
            get;
        }

        public void ListenFirstPlayerCommand()
        {
            var stream = this.FirstUser.Client.GetStream();

            while (true)
            {
                var buffer = new byte[1024];
                var bytes = stream.Read(buffer, 0, buffer.Length);
                var message = Encoding.ASCII.GetString(buffer, 0, bytes);
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

        public void SendGameStateToClients(GameState state)
        {
            var data = this.SerializeGameStateToByteArray(state);
            var firstUserStream = this.FirstUser.Client.GetStream();
            // todo second user stream ...

            firstUserStream.Write(data, 0, data.Length);
            firstUserStream.Flush();
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
