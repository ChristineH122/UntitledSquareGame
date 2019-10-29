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
            this.client = new TcpClient();
            this.endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        public void Start()
        {
            this.client.Connect(this.endpoint);
        }
    }
}
