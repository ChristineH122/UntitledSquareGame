using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ConnectionHandler
    {
        private const string MOVE_UP_COMMAND = "move:up";

        private TcpClient client;
        private IPEndPoint endpoint;

        public ConnectionHandler(string ipAddress, int port)
        {
            this.endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.client = new TcpClient();
            this.client.Connect(this.endpoint);
        }

        public void Start()
        {
            var stream = client.GetStream();
            var message = Encoding.Default.GetBytes("Test");
            stream.Write(message, 0, message.Length);
            stream.Flush();
            // stream.Close();
        }

        public void MoveUp()
        {
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.Write(MOVE_UP_COMMAND);
            writer.Flush();
        }
    }
}
