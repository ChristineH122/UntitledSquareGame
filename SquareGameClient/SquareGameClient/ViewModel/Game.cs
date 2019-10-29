using SquareGameClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGameClient.ViewModel
{
    public class Game
    {
        ConnectionHandler connectionHandler;

        public Game()
        {
            this.connectionHandler = new ConnectionHandler();
        }
    }
}
