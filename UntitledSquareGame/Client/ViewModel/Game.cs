using Client.Commands;
using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class Game
    {
        private ConnectionHandler connectionHandler;

        public Command MoveUpCommand
        {
            get; set;
        }

        public Game()
        {
            this.MoveUpCommand = new Command(obj => this.Send());
            this.connectionHandler = new ConnectionHandler("10.13.202.37", 5050);
        }

        public void Send()
        {
            this.connectionHandler.Start();
        }
    }
}
