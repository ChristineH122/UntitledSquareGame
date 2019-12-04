using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Server.Model;

namespace Server
{
    public class Program
    {
        public static List<User> Users;

        static void Main(string[] args)
        {
            Users = new List<User>();
            var game = new Game();
            ListenForUsers();
        }

        public static void ListenForUsers()
        {
            var listener = new TcpListener(new IPEndPoint(IPAddress.Any, 5050));
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                var user = new User(client);
                Users.Add(user);
                //var secondClient = listener.AcceptTcpClient();
                //var user2 = new User(secondClient);
                var session = new Session(user);
                //var session = new Session(user, user2);
            }
        }
    }
}