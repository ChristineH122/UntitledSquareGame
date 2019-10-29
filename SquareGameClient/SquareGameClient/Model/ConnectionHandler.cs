using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SquareGameClient.Model
{
    public class ConnectionHandler
    {
        private TcpClient client;
        private IPEndPoint endpoint;

        public ConnectionHandler(string ipAddress, int port)
        {
            this.endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            this.client = new TcpClient();
        }

        public void Start()
        {
            this.client.Connect(this.endpoint);
            var stream = client.GetStream();
            var message = Encoding.Default.GetBytes("Test");
            stream.Write(message, 0, message.Length);
            stream.Flush();
            stream.Close();
        }
    }
}
