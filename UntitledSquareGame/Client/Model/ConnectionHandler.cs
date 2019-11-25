using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ConnectionHandler
    {
        private TcpClient client;
        private IPEndPoint endpoint;

        public ConnectionHandler(string ipAddress, int port)
        {
            this.endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
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
    }
}
