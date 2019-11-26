using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class User
    {
        public User(TcpClient client)
        {
            this.Client = client;
        }

        public TcpClient Client
        {
            get;
        }
    }
}
