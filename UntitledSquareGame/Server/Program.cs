using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new TcpListener(new IPEndPoint(IPAddress.Any, 5050));
            listener.Start();
            var client = listener.AcceptTcpClient();

            var buffer = new byte[1024];
            var stream = client.GetStream();
            stream.Read(buffer, 0, 1024);

            var message = Encoding.ASCII.GetString(buffer);
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}